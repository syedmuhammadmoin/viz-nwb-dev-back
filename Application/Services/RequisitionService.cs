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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RequisitionService : IRequisitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequisitionService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginationResponse<List<RequisitionDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var docDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                docDate.Add(filter.DocDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }

            var requisition = await _unitOfWork.Requisition.GetAll(new RequisitionSpecs(docDate, states, filter, false));

            if (requisition.Count() == 0)
                return new PaginationResponse<List<RequisitionDto>>(_mapper.Map<List<RequisitionDto>>(requisition), "List is empty");

            var totalRecords = await _unitOfWork.Requisition.TotalRecord(new RequisitionSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<RequisitionDto>>(_mapper.Map<List<RequisitionDto>>(requisition),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<RequisitionDto>> GetByIdAsync(int id)
        {
            var specification = new RequisitionSpecs(false);
            var requisition = await _unitOfWork.Requisition.GetById(id, specification);
            if (requisition == null)
                return new Response<RequisitionDto>("Not found");

            var requisitionProductItemIds = requisition.RequisitionLines.Select(i => i.ItemId).ToList();
            //Getting stock by items id
            var stock = await _unitOfWork.Stock.GetAll(new StockSpecs(requisitionProductItemIds));

            var requisitionDto = _mapper.Map<RequisitionDto>(requisition);
            List<int> reqQuantity = new List<int>();
            List<decimal> amounts = new List<decimal>();
            foreach (var line in requisitionDto.RequisitionLines)
            {
                line.AvailableQuantity = stock
                    .Where(i => i.ItemId == line.ItemId && i.WarehouseId == line.WarehouseId)
                    .Sum(i => i.AvailableQuantity);

                line.SubTotal = line.PurchasePrice * line.Quantity;

                if (line.AvailableQuantity >= 0)
                {
                    if(line.AvailableQuantity >= 1)
                    {
                        requisitionDto.IsShowIssuanceButton = true;
                    }
                    if (line.Quantity >= line.AvailableQuantity)
                    {
                        var requiredQuantity = line.Quantity - line.AvailableQuantity;
                        reqQuantity.Add(requiredQuantity);
                        var calculateAmount = requiredQuantity * line.PurchasePrice;
                        var amount = calculateAmount;
                        amounts.Add(amount);
                    }
                }
            }

            ReturningRemarks(requisitionDto);
            var totalAmount = amounts.Sum();
            if (reqQuantity.Count() > 0)
            {
                if (totalAmount <= 100000)
                {
                    requisitionDto.IsShowPurchaseOrderButton = true;
                }
                else if (totalAmount > 100000 && totalAmount <= 300000)
                {
                    requisitionDto.IsShowCFQButton = true;
                }
                else if (totalAmount > 300000)
                {
                    requisitionDto.IsShowTenderButton = true;
                }
            }
            if ((requisitionDto.State == DocumentStatus.Unpaid || requisitionDto.State == DocumentStatus.Partial || requisitionDto.State == DocumentStatus.Paid))
                return new Response<RequisitionDto>(MapToValue(requisitionDto), "Returning value");

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Requisition)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == requisitionDto.StatusId));
                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                            requisitionDto.IsAllowedRole = true;
                    }
                }
            }
            return new Response<RequisitionDto>(requisitionDto, "Returning value");
        }

        public async Task<Response<List<RequisitionDropDownDto>>> GetRequisitionDropDown()
        {
            var requisition = await _unitOfWork.Requisition.GetAll(new RequisitionSpecs(0));
            if (!requisition.Any())
                return new Response<List<RequisitionDropDownDto>>("List is empty");

            return new Response<List<RequisitionDropDownDto>>(_mapper.Map<List<RequisitionDropDownDto>>(requisition), "Returning List");
        }
        
        public async Task<Response<RequisitionDto>> CreateAsync(CreateRequisitionDto entity)
        {
            if ((bool)entity.isSubmit)
            {

                return await this.SubmitRequisition(entity);
            }
            else
            {

                return await this.SaveRequisition(entity, 1);
            }
        }
        
        public async Task<Response<RequisitionDto>> UpdateAsync(CreateRequisitionDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitRequisition(entity);
            }
            else
            {
                return new Response<RequisitionDto>("Not allow to edit");
                //return await this.UpdateRequisition(entity, 1);
            }
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getRequisition = await _unitOfWork.Requisition.GetById(data.DocId, new RequisitionSpecs(true));
            if (getRequisition == null)
                return new Response<bool>("Requisition with the input id not found");
            
            if (getRequisition.Status.State == DocumentStatus.Unpaid || getRequisition.Status.State == DocumentStatus.Partial || getRequisition.Status.State == DocumentStatus.Paid)
                return new Response<bool>("Requisition already approved");
            
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Requisition)).FirstOrDefault();
            if (workflow == null)
                return new Response<bool>("No activated workflow found for this document");
            
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getRequisition.StatusId && x.Action == data.Action));
            if (transition == null)
                return new Response<bool>("No transition found");
            
            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
           
            _unitOfWork.CreateTransaction();
            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getRequisition.setStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getRequisition.Id,
                            DocType = DocType.Requisition,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }
                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        foreach (var line in getRequisition.RequisitionLines)
                        {
                            line.setStatus(DocumentStatus.Unreconciled);
                        }

                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Requisition Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        foreach (var line in getRequisition.RequisitionLines)
                        {
                            var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs(line.ItemId, (int)line.WarehouseId)).FirstOrDefault();
                            getStockRecord.updateRequisitionReservedQuantity(getStockRecord.ReservedRequisitionQuantity - line.ReserveQuantity);
                            getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity + line.ReserveQuantity);
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Requisition Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Requisition Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");


        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<Response<RequisitionDto>> SubmitRequisition(CreateRequisitionDto entity)
        {
            int statusId = (bool)entity.IsWithoutWorkflow ? 3 : 6;
            if (!(bool)entity.IsWithoutWorkflow)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Requisition)).FirstOrDefault();
                if (checkingActiveWorkFlows == null)
                    return new Response<RequisitionDto>("No workflow found for Requisition");
            }
            //this code is not support for editable Requisition
            //Checking available quantity in stock
            var requisition = CheckOrUpdateQty(entity);

            //this code is not support for editable Requisition
            if (entity.Id == null)
            {
                return await this.SaveRequisition(requisition, statusId);
            }
            else
            {
                return await this.UpdateRequisition(requisition, statusId);
            }
        }

        private CreateRequisitionDto CheckOrUpdateQty(CreateRequisitionDto entity)
        {
            foreach (var line in entity.RequisitionLines)
            {
                var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs((int)line.ItemId, (int)line.WarehouseId)).FirstOrDefault();
                if (getStockRecord.AvailableQuantity > 0)
                {
                    int reserveableQuantity = (int)line.Quantity;
                    
                    if (line.Quantity > getStockRecord.AvailableQuantity)
                        reserveableQuantity = getStockRecord.AvailableQuantity;
                    
                    //Need to Save reserveable Quantity with Requesition
                    line.ReserveQuantity = reserveableQuantity;
                    getStockRecord.updateRequisitionReservedQuantity(getStockRecord.ReservedRequisitionQuantity + reserveableQuantity);
                    getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity - reserveableQuantity);
                }
            }
            return entity;
        }

        private async Task<Response<RequisitionDto>> SaveRequisition(CreateRequisitionDto entity, int status)
        {
            if (entity.RequisitionLines.Count() == 0)
                return new Response<RequisitionDto>("Lines are required");

            //Checking duplicate Lines if any
            var duplicates = entity.RequisitionLines.GroupBy(x => new { x.ItemId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<RequisitionDto>("Duplicate Lines found");

            var requisition = _mapper.Map<RequisitionMaster>(entity);
            
            //Setting status
            requisition.setStatus(status);

            _unitOfWork.CreateTransaction();
            //Saving in table
            var result = await _unitOfWork.Requisition.Add(requisition);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            requisition.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<RequisitionDto>(_mapper.Map<RequisitionDto>(result), "Created successfully");
        }

        private async Task<Response<RequisitionDto>> UpdateRequisition(CreateRequisitionDto entity, int status)
        {
            if (entity.RequisitionLines.Count() == 0)
                return new Response<RequisitionDto>("Lines are required");
            
            //Checking duplicate Lines if any
            var duplicates = entity.RequisitionLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<RequisitionDto>("Duplicate Lines found");

            var requisition = await _unitOfWork.Requisition.GetById((int)entity.Id, new RequisitionSpecs(true));
            if (requisition == null)
                return new Response<RequisitionDto>("Not found");

            if (requisition.StatusId != 1 && requisition.StatusId != 2)
                return new Response<RequisitionDto>("Only draft document can be edited");

            //Setting status
            requisition.setStatus(status);

            _unitOfWork.CreateTransaction();
            //For updating data
            _mapper.Map<CreateRequisitionDto, RequisitionMaster>(entity, requisition);
            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<RequisitionDto>(_mapper.Map<RequisitionDto>(requisition), "Updated successfully");
        }

        private RequisitionDto MapToValue(RequisitionDto data)
        {
            //Get reconciled issuances
            var grnLineReconcileRecord = _unitOfWork.RequisitionToIssuanceLineReconcile
                .Find(new RequisitionToIssuanceLineReconcileSpecs(true, data.Id))
                .GroupBy(x => new { x.IssuanceId, x.Issuance.DocNo })
                .Where(g => g.Count() >= 1)
                .Select(y => new
                {
                    IssuanceId = y.Key.IssuanceId,
                    DocNo = y.Key.DocNo,
                })
                .ToList();

            // Adding in issuances in references list
            var getReference = new List<ReferncesDto>();
            if (grnLineReconcileRecord.Any())
            {
                foreach (var line in grnLineReconcileRecord)
                {
                    getReference.Add(new ReferncesDto
                    {
                        DocId = line.IssuanceId,
                        DocNo = line.DocNo,
                        DocType = DocType.Issuance
                    });
                }
            }
            data.References = getReference;

            // Get pending & received quantity...
            foreach (var line in data.RequisitionLines)
            {
                // Checking if given amount is greater than unreconciled document amount
                line.IssuedQuantity = _unitOfWork.RequisitionToIssuanceLineReconcile
                    .Find(new RequisitionToIssuanceLineReconcileSpecs(data.Id, line.Id, line.ItemId))
                    .Sum(p => p.Quantity);

                line.PendingQuantity = line.Quantity - line.IssuedQuantity;
            }

            return data;
        }

        //private List<decimal> ValidateProcurementProcess(RequisitionDto data )
        //{
        //    List<decimal> decimals = new List<decimal>();
        //    foreach (var line in data.RequisitionLines)
        //    {
        //        var getStock = _unitOfWork.Stock.Find(new StockSpecs((int)line.ItemId, (int)line.WarehouseId)).FirstOrDefault();
        //        if (getStock.AvailableQuantity > 0)
        //            data.IsShowIssuanceButton = true;
        //        var reservedQuantity = line.Quantity - getStock.AvailableQuantity;
        //        var  calculateAmount = reservedQuantity * line.PurchasePrice;
                
        //        var amount = calculateAmount;
           
        //        decimals.Add(amount);
        //    }


        //    return decimals;

        //}
       
        private List<RemarksDto> ReturningRemarks(RequisitionDto data)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Requisition))
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
