﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class DisposalService : IDisposalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DisposalService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<DisposalDto>> CreateAsync(CreateDisposalDto entity)
        {

            if ((bool)entity.IsSubmit)
            {
                return await Submit(entity);
            }
            else
            {
                return await Save(entity, 1);
            }
        }

        public async Task<Response<DisposalDto>> UpdateAsync(CreateDisposalDto entity)
        {
            if ((bool)entity.IsSubmit)
            {
                return await Submit(entity);
            }
            else
            {
                return await Update(entity, 1);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<DisposalDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var disposal = await _unitOfWork.Disposal.GetAll(new DisposalSpecs(filter, false));
            if (disposal.Count() == 0)
                return new PaginationResponse<List<DisposalDto>>(_mapper.Map<List<DisposalDto>>(disposal), "List is empty");

            var totalRecords = await _unitOfWork.Disposal.TotalRecord(new DisposalSpecs(filter, true));

            return new PaginationResponse<List<DisposalDto>>(_mapper.Map<List<DisposalDto>>(disposal),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DisposalDto>> GetByIdAsync(int id)
        {
            var disposal = await _unitOfWork.Disposal.GetById(id, new DisposalSpecs());
            if (disposal == null)
                return new Response<DisposalDto>("Not found");
            
            var disposalDto = _mapper.Map<DisposalDto>(disposal);

            ReturningRemarks(disposalDto);
            disposalDto.IsAllowedRole = false;
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Disposal)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == disposalDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            disposalDto.IsAllowedRole = true;
                        }
                    }
                }
            }

            return new Response<DisposalDto>(disposalDto, "Returning value");
        }

        //private async Task AddToLedger(Disposal bill)
        //{
        //    var transaction = new Transactions(bill.Id, bill.DocNo, DocType.Disposal);
        //    await _unitOfWork.Transaction.Add(transaction);
        //    await _unitOfWork.SaveAsync();

        //    bill.SetTransactionId(transaction.Id);
        //    await _unitOfWork.SaveAsync();

        //    //Inserting line amount into recordledger table
           
               

        //        var drAccumulatedDepreciation = new RecordLedger(
        //            transaction.Id,
        //            line.AccountId,
        //            bill.VendorId,
        //            line.WarehouseId,
        //            line.Description,
        //            'D',
        //            amount + tax + line.AnyOtherTax,
        //            bill.CampusId,
        //            bill.BillDate
        //            );

        //        await _unitOfWork.Ledger.Add(drAccumulatedDepreciation);
        //        await _unitOfWork.SaveAsync();


            
        //    var getVendorAccount = await _unitOfWork.BusinessPartner.GetById(bill.VendorId);
        //    var addPayableInLedger = new RecordLedger(
        //                transaction.Id,
        //                (Guid)getVendorAccount.AccountPayableId,
        //                bill.VendorId,
        //                null,
        //                bill.DocNo,
        //                'C',
        //                bill.TotalAmount,
        //                bill.CampusId,
        //                bill.BillDate
        //            );

        //    await _unitOfWork.Ledger.Add(addPayableInLedger);
        //    await _unitOfWork.SaveAsync();

        //    //Getting transaction with Payment Transaction Id
        //    var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(transaction.Id, true)).FirstOrDefault();

        //    bill.SetLedgerId(getUnreconciledDocumentAmount.Id);
        //    await _unitOfWork.SaveAsync();
        //}

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getDisposal = await _unitOfWork.Disposal.GetById(data.DocId, new DisposalSpecs());
            if (getDisposal == null)
            {
                return new Response<bool>("Disposal with the input id not found");
            }
            
            if (getDisposal.Status.State == DocumentStatus.Unpaid || getDisposal.Status.State == DocumentStatus.Partial || getDisposal.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Disposal already approved");
            }
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Disposal)).FirstOrDefault();
            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getDisposal.StatusId && x.Action == data.Action));
            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }

            // Creating object of getUSer class
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(_httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();

            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getDisposal.SetStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getDisposal.Id,
                            DocType = DocType.Disposal,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        var getFixedAsset = await _unitOfWork.FixedAsset.GetById(getDisposal.FixedAssetId);
                        getFixedAsset.SetIsDisposedTrue();
                        // Ledger Entry
                        //await AddToLedger();
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Document Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Document Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Document Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");
        }
        
        //Private methods
        private async Task<Response<DisposalDto>> Submit(CreateDisposalDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Disposal)).FirstOrDefault();
            if (checkingActiveWorkFlows == null)
            {
                return new Response<DisposalDto>("No workflow found for Disposal");
            }

            if (entity.Id == null)
            {
                return await this.Save(entity, 6);
            }
            else
            {
                return await this.Update(entity, 6);
            }
        }

        private async Task<Response<DisposalDto>> Save(CreateDisposalDto entity, int status)
        {
            //Getting fixed asset
            var getFixedAsset = await _unitOfWork.FixedAsset.GetById((int)entity.FixedAssetId);
            if(getFixedAsset == null)
                return new Response<DisposalDto>("Invalid fixed asset id");

            //Setting values in disposal
            var disposal = new Disposal((int)entity.FixedAssetId, getFixedAsset.ProductId, getFixedAsset.Cost,
                getFixedAsset.SalvageValue, (int)getFixedAsset.UseFullLife, (Guid)getFixedAsset.AccumulatedDepreciationId,
                0, entity.DisposalDate, entity.DisposalValue, getFixedAsset.WarehouseId, status);

            //Saving in table
            _unitOfWork.CreateTransaction();
            await _unitOfWork.Disposal.Add(disposal);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            disposal.CreateCode();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<DisposalDto>(_mapper.Map<DisposalDto>(disposal), "Created successfully");
        }

        private async Task<Response<DisposalDto>> Update(CreateDisposalDto entity, int status)
        {
            //Getting disposal by id
            var result = await _unitOfWork.Disposal.GetById((int)entity.Id);
            if (result == null)
                return new Response<DisposalDto>("Not found");
            
            //Checking status
            if (result.StatusId != 1 && result.StatusId != 2)
                return new Response<DisposalDto>("Only draft document can be edited");

            //Getting fixed asset
            var getFixedAsset = await _unitOfWork.FixedAsset.GetById((int)entity.FixedAssetId);
            if (getFixedAsset == null)
                return new Response<DisposalDto>("Invalid fixed asset id");

            //Updating disposal
            result.Update((int)entity.FixedAssetId, getFixedAsset.ProductId, getFixedAsset.Cost,
                getFixedAsset.SalvageValue, (int)getFixedAsset.UseFullLife, (Guid)getFixedAsset.AccumulatedDepreciationId,
                0, entity.DisposalDate, entity.DisposalValue, getFixedAsset.WarehouseId, status);

            //saving data
            await _unitOfWork.SaveAsync();

            //returning response
            return new Response<DisposalDto>(_mapper.Map<DisposalDto>(result), "Updated successfully");
        }

        private List<RemarksDto> ReturningRemarks(DisposalDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Disposal))
                    .Select(e => new RemarksDto()
                    {
                        Remarks = e.Remarks,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (remarks.Count() > 0)
            {
                data.RemarksList = _mapper.Map<List<RemarksDto>>(remarks);
            }
            return remarks;
        }

    }
}