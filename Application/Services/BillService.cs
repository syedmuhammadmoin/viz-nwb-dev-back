using Application.Contracts.DTOs;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BillService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Response<BillDto>> CreateAsync(CreateBillDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitBILL(entity);
            }
            else
            {
                return await this.SaveBILL(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<BillDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var docDate = new List<DateTime?>();
            var dueDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.DueDate != null)
            {
                dueDate.Add(filter.DueDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }

            var bills = await _unitOfWork.Bill.GetAll(new BillSpecs(docDate, dueDate, states, filter, false));

            if (bills.Count() == 0)
                return new PaginationResponse<List<BillDto>>(_mapper.Map<List<BillDto>>(bills), "List is empty");

            var totalRecords = await _unitOfWork.Bill.TotalRecord(new BillSpecs(docDate, dueDate, states, filter, true));

            return new PaginationResponse<List<BillDto>>(_mapper.Map<List<BillDto>>(bills),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BillDto>> GetByIdAsync(int id)
        {
            var specification = new BillSpecs(false);
            var bill = await _unitOfWork.Bill.GetById(id, specification);
            if (bill == null)
                return new Response<BillDto>("Not found");

            var billDto = _mapper.Map<BillDto>(bill);
            ReturningRemarks(billDto, DocType.Bill);



            ReturningFiles(billDto, DocType.Bill);


            if ((billDto.State == DocumentStatus.Unpaid || billDto.State == DocumentStatus.Partial || billDto.State == DocumentStatus.Paid) && billDto.TransactionId != null)
            {
                return new Response<BillDto>(MapToValue(billDto), "Returning value");
            }

            billDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Bill)).FirstOrDefault();

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == billDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            billDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<BillDto>(billDto, "Returning value");
        }

        public async Task<Response<BillDto>> UpdateAsync(CreateBillDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitBILL(entity);
            }
            else
            {
                return await this.UpdateBILL(entity, 1);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getBill = await _unitOfWork.Bill.GetById(data.DocId, new BillSpecs(true));

            if (getBill == null)
            {
                return new Response<bool>("Bill with the input id not found");
            }
            if (getBill.Status.State == DocumentStatus.Unpaid || getBill.Status.State == DocumentStatus.Partial || getBill.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Bill already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Bill)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getBill.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            // Creating object of getUSer class
            var getUser = new GetUser(this._httpContextAccessor);

            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();

            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getBill.setStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getBill.Id,
                            DocType = DocType.Bill,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }


                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        await AddToLedger(getBill);
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Bill Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Bill Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Bill Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");

        }

        //Private Methods for Bills
        private async Task<Response<BillDto>> SubmitBILL(CreateBillDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Bill)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<BillDto>("No workflow found for Bill");
            }

            if (entity.Id == null)
            {
                return await this.SaveBILL(entity, 6);
            }
            else
            {
                return await this.UpdateBILL(entity, 6);
            }
        }

        private async Task<Response<BillDto>> SaveBILL(CreateBillDto entity, int status)
        {
            if (entity.BillLines.Count() == 0)
                return new Response<BillDto>("Lines are required");

            var bill = _mapper.Map<BillMaster>(entity);

            //setting BusinessPartnerPayable
            
            var businessPartner = await _unitOfWork.BusinessPartner.GetById((int)entity.VendorId);

            // checking if employee is business partner
            if (businessPartner.BusinessPartnerType == BusinessPartnerType.Employee)
            {
                //checking if account payable has been assigned to employee

                if (businessPartner.AccountPayableId == null)
                    return new Response<BillDto>("Payable account not found for the business partner");
            }
            
            //Validation for Payable and Receivable
            foreach (var check in entity.BillLines)
            {
                var level4 =await _unitOfWork.Level4.GetById((Guid) check.AccountId);

                var level3 = ReceivableAndPayable.Validate(level4.Level3_id);

                if (level3 == false)
                    return new Response<BillDto>("Account Invalid");
            }
            bill.setPayableAccountId((Guid)businessPartner.AccountPayableId);

            //Setting status
            bill.setStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.Bill.Add(bill);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            bill.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<BillDto>(_mapper.Map<BillDto>(result), "Created successfully");

        }

        private async Task<Response<BillDto>> UpdateBILL(CreateBillDto entity, int status)
        {
            if (entity.BillLines.Count() == 0)
                return new Response<BillDto>("Lines are required");

            var specification = new BillSpecs(true);
            var bill = await _unitOfWork.Bill.GetById((int)entity.Id, specification);

            if (bill == null)
                return new Response<BillDto>("Not found");

            if (bill.StatusId != 1 && bill.StatusId != 2)
                return new Response<BillDto>("Only draft document can be edited");

            //setting BusinessPartnerPayable
            var businessPartner = await _unitOfWork.BusinessPartner.GetById((int)entity.VendorId);

            if (businessPartner.BusinessPartnerType == BusinessPartnerType.Employee)
            {
                //checking if account payable has been assigned to employee
                if (businessPartner.AccountPayableId == null)
                    return new Response<BillDto>("Payable account not found for the business partner");
            }

            //Validation for Payable and Receivable
            foreach (var check in entity.BillLines)
            {
                var level4 = await _unitOfWork.Level4.GetById((Guid)check.AccountId);

                var level3 = ReceivableAndPayable.Validate(level4.Level3_id);

                if (level3 == false)
                    return new Response<BillDto>("Account Invalid");
            }

            bill.setPayableAccountId((Guid)businessPartner.AccountPayableId);


            bill.setStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateBillDto, BillMaster>(entity, bill);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<BillDto>(_mapper.Map<BillDto>(bill), "Created successfully");

        }

        private async Task AddToLedger(BillMaster bill)
        {
            var transaction = new Transactions(bill.Id, bill.DocNo, DocType.Bill);
            await _unitOfWork.Transaction.Add(transaction);
            await _unitOfWork.SaveAsync();

            bill.setTransactionId(transaction.Id);
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
                    line.WarehouseId,
                    line.Description,
                    'D',
                    amount + tax + line.AnyOtherTax,
                    bill.CampusId,
                    bill.BillDate
                    );

                await _unitOfWork.Ledger.Add(addSalesAmountInRecordLedger);
                await _unitOfWork.SaveAsync();


            }
            var getVendorAccount = await _unitOfWork.BusinessPartner.GetById(bill.VendorId);
            var addPayableInLedger = new RecordLedger(
                        transaction.Id,
                        (Guid)getVendorAccount.AccountPayableId,
                        bill.VendorId,
                        null,
                        bill.DocNo,
                        'C',
                        bill.TotalAmount,
                        bill.CampusId,
                        bill.BillDate
                    );

            await _unitOfWork.Ledger.Add(addPayableInLedger);
            await _unitOfWork.SaveAsync();

            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs(transaction.Id, true)).FirstOrDefault();

            bill.setLedgerId(getUnreconciledDocumentAmount.Id);
            await _unitOfWork.SaveAsync();
        }

        private BillDto MapToValue(BillDto data)
        {
            //Getting transaction with Payment Transaction Id
            var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)data.TransactionId, true)).FirstOrDefault();

            // Checking if given amount is greater than unreconciled document amount
            var transactionReconciles = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false)).ToList();

            //For Paid Document List
            var paidDocList = new List<PaidDocListDto>();
            // if reconciles transaction found
            if (transactionReconciles.Count() > 0)
            {
                //Adding Paid Doc List
                foreach (var tranRecon in transactionReconciles)
                {
                    paidDocList.Add(new PaidDocListDto
                    {
                        Id = tranRecon.PaymentLedger.Transactions.DocId,
                        DocNo = tranRecon.PaymentLedger.Transactions.DocNo,
                        DocType = tranRecon.PaymentLedger.Transactions.DocType,
                        Amount = tranRecon.Amount
                    });
                }
            }
            // Remarks 
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Bill))
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


            //Getting Pending Invoice Amount
            var pendingAmount = data.TotalAmount - transactionReconciles.Sum(e => e.Amount);

            if (data.State != DocumentStatus.Paid)
            {
                //Creating transactionReconRepo object
                TransactionReconcileService trasactionReconService = new TransactionReconcileService(_unitOfWork);
                var getUnreconPayment = trasactionReconService.GetPaymentReconAmounts(getUnreconciledDocumentAmount.Level4_id, (int)getUnreconciledDocumentAmount.BusinessPartnerId, getUnreconciledDocumentAmount.Sign);


                //For Getting Business Partner Unreconciled Payments and CreditNote
                var BPUnreconPayments = getUnreconPayment.Result.Select(i => new UnreconciledBusinessPartnerPaymentsDto()
                {
                    Id = i.DocId,
                    DocNo = i.DocNo,
                    DocType = i.DocType,
                    Amount = i.UnreconciledAmount,
                    PaymentLedgerId = i.PaymentLedgerId
                }).ToList();

                data.BPUnreconPaymentList = BPUnreconPayments;
            }


            data.TotalPaid = transactionReconciles.Sum(e => e.Amount);
            data.PaidAmountList = paidDocList;
            data.PendingAmount = pendingAmount;

            // Returning BillDto with all values assigned
            return data;
        }

        public async Task<Response<List<BillDto>>> GetAgingReport()
        {
            var bills = await _unitOfWork.Bill.GetAll(new BillSpecs(""));

            if (bills.Count() == 0)
                return new PaginationResponse<List<BillDto>>(_mapper.Map<List<BillDto>>(bills), "List is empty");

            var response = new List<BillDto>();
            var billDto = _mapper.Map<List<BillDto>>(bills);
            foreach (var i in billDto)
            {
                //Getting transaction with Payment Transaction Id
                var getUnreconciledDocumentAmount = _unitOfWork.Ledger.Find(new LedgerSpecs((int)i.TransactionId, true)).FirstOrDefault();

                // Checking if given amount is greater than unreconciled document amount
                var transactionReconciles = _unitOfWork.TransactionReconcile.Find(new TransactionReconSpecs(getUnreconciledDocumentAmount.Id, false)).ToList();

                //Getting Pending Invoice Amount
                i.PendingAmount = i.TotalAmount - transactionReconciles.Sum(e => e.Amount);

                //response.Add(await MapToValue(i));
            }
            return new Response<List<BillDto>>(billDto, "Returning Report");
        }

        private List<RemarksDto> ReturningRemarks(BillDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Bill))
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

        private List<FileUploadDto> ReturningFiles(BillDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.Bill))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.DebitNote,
                        Extension = e.Extension,
                        UserName = e.User.UserName,
                        CreatedAt = e.CreatedDate == null ? "N/A" : ((DateTime)e.CreatedDate).ToString("ddd, dd MMM yyyy")
                    }).ToList();

            if (files.Count() > 0)
            {
                data.FileUploadList = _mapper.Map<List<FileUploadDto>>(files);

            }

            return files;
        }     
    }
}
