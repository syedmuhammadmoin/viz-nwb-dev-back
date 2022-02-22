using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<BillDto>> CreateAsync(CreateBillDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitBILL(entity);
            }
            else
            {
                return await this.SaveBILL(entity, DocumentStatus.Draft);
            }
        }

        public async Task<PaginationResponse<List<BillDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new BillSpecs(filter);
            var bills = await _unitOfWork.Bill.GetAll(specification);

            if (bills.Count() == 0)
                return new PaginationResponse<List<BillDto>>("List is empty");

            var totalRecords = await _unitOfWork.Bill.TotalRecord();

            return new PaginationResponse<List<BillDto>>(_mapper.Map<List<BillDto>>(bills),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BillDto>> GetByIdAsync(int id)
        {
            var specification = new BillSpecs(false);
            var bill = await _unitOfWork.Bill.GetById(id, specification);
            if (bill == null)
                return new Response<BillDto>("Not found");

            return new Response<BillDto>(_mapper.Map<BillDto>(bill), "Returning value");
        }

        public async Task<Response<BillDto>> UpdateAsync(CreateBillDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitBILL(entity);
            }
            else
            {
                return await this.UpdateBILL(entity, DocumentStatus.Draft);
            }
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private Methods for Bills
        private async Task<Response<BillDto>> SubmitBILL(CreateBillDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveBILL(entity, DocumentStatus.Submitted);
            }
            else
            {
                return await this.UpdateBILL(entity, DocumentStatus.Submitted);
            }
        }

        private async Task<Response<BillDto>> SaveBILL(CreateBillDto entity, DocumentStatus status)
        {
            if (entity.BillLines.Count() == 0)
                return new Response<BillDto>("Lines are required");

            var bill = _mapper.Map<BillMaster>(entity);

            //Setting status
            bill.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.Bill.Add(bill);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                bill.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Adding bill to Ledger
                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(bill);
                }

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<BillDto>(_mapper.Map<BillDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<BillDto>(ex.Message);
            }
        }
        private async Task<Response<BillDto>> UpdateBILL(CreateBillDto entity, DocumentStatus status)
        {
            if (entity.BillLines.Count() == 0)
                return new Response<BillDto>("Lines are required");

            var specification = new BillSpecs(true);
            var bill = await _unitOfWork.Bill.GetById((int)entity.Id, specification);

            if (bill == null)
                return new Response<BillDto>("Not found");

            if (bill.Status == DocumentStatus.Submitted)
                return new Response<BillDto>("Bill already submitted");

            bill.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateBillDto, BillMaster>(entity, bill);

                await _unitOfWork.SaveAsync();

                //Adding bill to Ledger
                if (status == DocumentStatus.Submitted)
                {
                    await AddToLedger(bill);
                }

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<BillDto>(_mapper.Map<BillDto>(bill), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<BillDto>(ex.Message);
            }
        }
        private async Task AddToLedger(BillMaster bill)
        {
            var transaction = new Transactions(bill.DocNo, DocType.Bill);
            var addTransaction = await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            bill.setTrasactionId(transaction.Id);
            await _unitOfWork.SaveAsync();

            //Inserting line amount into recordledger table
            foreach (var line in bill.BillLines)
            {
                var tax = (line.Quantity * line.Cost * line.Tax) / 100;
                var amount = line.Quantity * line.Cost;

                var addSalesAmountInRecordLedger = new RecordLedger(
                    transaction.Id,
                    line.AccountId,
                    bill.VendorId,
                    line.LocationId,
                    line.Description,
                    'D',
                    amount+tax
                    );

                await _unitOfWork.Ledger.Add(addSalesAmountInRecordLedger);
                await _unitOfWork.SaveAsync();


            }
            var getVendorAccount = await _unitOfWork.BusinessPartner.GetById(bill.VendorId);
            var addPayableInLedger = new RecordLedger(
                        transaction.Id,
                        getVendorAccount.AccountPayableId,
                        bill.VendorId,
                        null,
                        bill.DocNo,
                        'C',
                        bill.TotalAmount
                    );

            await _unitOfWork.Ledger.Add(addPayableInLedger);
            await _unitOfWork.SaveAsync();
        }
    }
}
