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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class IssuanceService : IIssuanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IssuanceService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<IssuanceDto>> CreateAsync(CreateIssuanceDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitIssuance(entity);
            }
            else
            {
                return await this.SaveIssuance(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<IssuanceDto>>> GetAllAsync(TransactionFormFilter filter)
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
            var issuances = await _unitOfWork.Issuance.GetAll(new IssuanceSpecs(docDate, states, filter, false));

            if (issuances.Count() == 0)
                return new PaginationResponse<List<IssuanceDto>>(_mapper.Map<List<IssuanceDto>>(issuances), "List is empty");

            var totalRecords = await _unitOfWork.Issuance.TotalRecord(new IssuanceSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<IssuanceDto>>(_mapper.Map<List<IssuanceDto>>(issuances),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<IssuanceDto>> GetByIdAsync(int id)
        {
            var specification = new IssuanceSpecs(false);
            var issuance = await _unitOfWork.Issuance.GetById(id, specification);
            if (issuance == null)
                return new Response<IssuanceDto>("Not found");

            var issuanceDto = _mapper.Map<IssuanceDto>(issuance);
            ReturningRemarks(issuanceDto, DocType.Issuance);

            if ((issuanceDto.State == DocumentStatus.Partial || issuanceDto.State == DocumentStatus.Paid))
            {
                return new Response<IssuanceDto>(MapToValue(issuanceDto), "Returning value");

            }

            issuanceDto.IsAllowedRole = false;

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Issuance)).FirstOrDefault();
            //if ((issuanceDto.State == DocumentStatus.Unpaid || issuanceDto.State == DocumentStatus.Partial || issuanceDto.State == DocumentStatus.Paid))
            //{
            //    return new Response<IssuanceDto>(issuanceDto, "Returning value");
            //}

            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == issuanceDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            issuanceDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<IssuanceDto>(issuanceDto, "Returning value");
        }

        public async Task<Response<IssuanceDto>> UpdateAsync(CreateIssuanceDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitIssuance(entity);
            }
            else
            {
                return await this.UpdateIssuance(entity, 1);
            }
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getIssuance = await _unitOfWork.Issuance.GetById(data.DocId, new IssuanceSpecs(true));
            if (getIssuance == null)
                return new Response<bool>("Issuance with the input id not found");

            if (getIssuance.Status.State == DocumentStatus.Unpaid || getIssuance.Status.State == DocumentStatus.Partial || getIssuance.Status.State == DocumentStatus.Paid)
                return new Response<bool>("Issuance already approved");

            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Issuance)).FirstOrDefault();
            if (workflow == null)
                return new Response<bool>("No activated workflow found for this document");

            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getIssuance.StatusId && x.Action == data.Action));
            if (transition == null)
                return new Response<bool>("No transition found");

            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles(); _unitOfWork.CreateTransaction();

            foreach (var role in currentUserRoles)
            {
                if (transition.AllowedRole.Name == role)
                {
                    getIssuance.setStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getIssuance.Id,
                            DocType = DocType.Issuance,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }

                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        foreach (var line in getIssuance.IssuanceLines)
                        {
                            line.setStatus(DocumentStatus.Unreconciled);
                        }

                        if (getIssuance.RequisitionId != null)
                        {
                            var reconciled = await ReconcileReqLines(getIssuance.Id, (int)getIssuance.RequisitionId, getIssuance.IssuanceLines);
                            if (!reconciled.IsSuccess)
                            {
                                _unitOfWork.Rollback();
                                return new Response<bool>(reconciled.Message);
                            }
                            await _unitOfWork.SaveAsync();
                        }

                        //updating reserved quantity in stock
                        var updateStockOnApproveOrReject = await UpdateStockOnApproveOrReject(getIssuance);
                        if (!updateStockOnApproveOrReject.IsSuccess)
                            return new Response<bool>(updateStockOnApproveOrReject.Message);


                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Issuance Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        //updating reserved quantity in stock
                        var updateStockOnApproveOrReject = await UpdateStockOnApproveOrReject(getIssuance);
                        if (!updateStockOnApproveOrReject.IsSuccess)
                            return new Response<bool>(updateStockOnApproveOrReject.Message);

                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Issuance Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "Issuance Reviewed");
                }
            }
            return new Response<bool>("User does not have allowed role");

        }

        //Private Methods for saving and updating Issuance
        private async Task<Response<IssuanceDto>> SaveIssuance(CreateIssuanceDto entity, int status)
        {
            if (entity.IssuanceLines.Count() == 0)
                return new Response<IssuanceDto>("Lines are required");

            if (entity.RequisitionId != null)
            {
                foreach (var issuanceLine in entity.IssuanceLines)
                {
                    //Getting Unreconciled Requisition lines
                    var getrequisitionLine = _unitOfWork.Requisition
                        .FindLines(new RequisitionLinesSpecs(issuanceLine.ItemId, (int)entity.RequisitionId ,(int)issuanceLine.WarehouseId, true))
                        .FirstOrDefault();

                    if (getrequisitionLine == null)
                        return new Response<IssuanceDto>("Selected item is not in requisition");

                    var checkValidation = CheckValidation((int)entity.RequisitionId, getrequisitionLine, _mapper.Map<IssuanceLines>(issuanceLine));
                    if (!checkValidation.IsSuccess)
                        return new Response<IssuanceDto>(checkValidation.Message);
                }
            }

            if (entity.RequisitionId == null)
            {  //Checking available quantity in stock
                var checkOrUpdateQty = CheckOrUpdateQty(entity);
                if (!checkOrUpdateQty.IsSuccess)
                    return new Response<IssuanceDto>(checkOrUpdateQty.Message);
            }
            else
            {//Checking available quantity in stock
                var checkOrUpdateQty = CheckOrUpdateQtyforRequisition(entity);
                if (!checkOrUpdateQty.IsSuccess)
                    return new Response<IssuanceDto>(checkOrUpdateQty.Message);
            }





            //Checking duplicate Lines if any
            var duplicates = entity.IssuanceLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<IssuanceDto>("Duplicate Lines found");

            var issuance = _mapper.Map<IssuanceMaster>(entity);

            //Setting status
            issuance.setStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.Issuance.Add(issuance);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            issuance.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<IssuanceDto>(_mapper.Map<IssuanceDto>(result), "Created successfully");

        }

        private async Task<Response<IssuanceDto>> SubmitIssuance(CreateIssuanceDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Issuance)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<IssuanceDto>("No workflow found for Issuance");
            }

            if (entity.Id == null)
            {
                return await this.SaveIssuance(entity, 6);
            }
            else
            {
                return await this.UpdateIssuance(entity, 6);
            }
        }

        private async Task<Response<IssuanceDto>> UpdateIssuance(CreateIssuanceDto entity, int status)
        {
            if (entity.IssuanceLines.Count() == 0)
                return new Response<IssuanceDto>("Lines are required");
            var getIssuance = await _unitOfWork.Issuance.GetById((int)entity.Id, new IssuanceSpecs(true));
            if (entity.RequisitionId != null)
            {

                foreach (var issuanceLine in entity.IssuanceLines)
                {
                    //Getting Unreconciled Requisition lines
                    var getrequisitionLine = _unitOfWork.Requisition
                         .FindLines(new RequisitionLinesSpecs(issuanceLine.ItemId, (int)entity.RequisitionId, (int)issuanceLine.WarehouseId, true))
                         .FirstOrDefault();


                    if (getrequisitionLine == null)
                        return new Response<IssuanceDto>("Selected item is not in requisition");

                    //var reconciled = await ReconcileReqLines(getIssuance.Id, (int)getIssuance.RequisitionId, getIssuance.IssuanceLines);

                    //if (!reconciled.IsSuccess)
                    //{
                    //    _unitOfWork.Rollback();
                    //    return new Response<IssuanceDto>(reconciled.Message);
                    //}
                    //await _unitOfWork.SaveAsync();
                    var checkValidation = CheckValidation((int)entity.RequisitionId, getrequisitionLine, _mapper.Map<IssuanceLines>(issuanceLine));
                    if (!checkValidation.IsSuccess)
                        return new Response<IssuanceDto>(checkValidation.Message);
                }
            }

            if (entity.RequisitionId == null)
            {  //Checking available quantity in stock
                var checkOrUpdateQty = CheckOrUpdateQty(entity);
                if (!checkOrUpdateQty.IsSuccess)
                    return new Response<IssuanceDto>(checkOrUpdateQty.Message);
            }
            else
            {//Checking available quantity in stock
                var checkOrUpdateQty = CheckOrUpdateQtyforRequisition(entity);
                if (!checkOrUpdateQty.IsSuccess)
                    return new Response<IssuanceDto>(checkOrUpdateQty.Message);
            }
            //if (entity.RequisitionId != null)
            //{

            //}

            //Checking duplicate Lines if any
            var duplicates = entity.IssuanceLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<IssuanceDto>("Duplicate Lines found");

            var specification = new IssuanceSpecs(true);
            var issuance = await _unitOfWork.Issuance.GetById((int)entity.Id, specification);

            if (issuance == null)
                return new Response<IssuanceDto>("Not found");

            if (issuance.StatusId != 1 && issuance.StatusId != 2)
                return new Response<IssuanceDto>("Only draft document can be edited");

            issuance.setStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateIssuanceDto, IssuanceMaster>(entity, issuance);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<IssuanceDto>(_mapper.Map<IssuanceDto>(issuance), "Updated successfully");

        }

        private Response<bool> CheckOrUpdateQty(CreateIssuanceDto issuance)
        {
            foreach (var line in issuance.IssuanceLines)
            {
                var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs((int)line.ItemId, (int)line.WarehouseId)).FirstOrDefault();

                if (getStockRecord == null)
                    return new Response<bool>("Item not found in stock");

                if (getStockRecord.AvailableQuantity == 0)
                    return new Response<bool>("Selected item is out of stock");


                if (line.Quantity > getStockRecord.AvailableQuantity)
                    return new Response<bool>("Selected item quantity is exceeding available quantity");


                getStockRecord.updateReservedQuantity(getStockRecord.ReservedQuantity + (int)line.Quantity);
                getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity - (int)line.Quantity);



            }
            return new Response<bool>(true, "");
        }
        private Response<bool> CheckOrUpdateQtyforRequisition(CreateIssuanceDto issuance)
        {
            foreach (var line in issuance.IssuanceLines)
            {
                var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs((int)line.ItemId, (int)line.WarehouseId)).FirstOrDefault();

                if (getStockRecord == null)
                    return new Response<bool>("Item not found in stock");

                if (getStockRecord.ReservedRequisitionQuantity == 0)
                    return new Response<bool>("Selected item is out of stock");


                if (line.Quantity > getStockRecord.ReservedRequisitionQuantity)
                    return new Response<bool>("Selected item quantity is exceeding available quantity");




            }
            return new Response<bool>(true, "");
        }

        private async Task<Response<bool>> UpdateStockOnApproveOrReject(IssuanceMaster issuance)
        {
            var getState = await _unitOfWork.WorkFlowStatus.GetById(issuance.StatusId);

            if (getState == null)
                return new Response<bool>("Invalid Status");

            foreach (var line in issuance.IssuanceLines)
            {
                var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs(line.ItemId, line.WarehouseId)).FirstOrDefault();

                if (getStockRecord == null)
                    return new Response<bool>("Item not found in stock");

                if (issuance.RequisitionId == null)
                {
                    // updating reserved quantity for APPROVED Issuance
                    if (getState.State == DocumentStatus.Unpaid)
                    {

                        getStockRecord.updateReservedQuantity(getStockRecord.ReservedQuantity - line.Quantity);
                    }

                    // updating reserved quantity for REJECTED Issuance
                    if (getState.State == DocumentStatus.Rejected)
                    {
                        getStockRecord.updateReservedQuantity(getStockRecord.ReservedQuantity - line.Quantity);
                        getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity + line.Quantity);
                    }
                }
                else
                {

                    RequisitionMaster getRequisition = await _unitOfWork.Requisition.GetById((int)issuance.RequisitionId, new RequisitionSpecs(false));

                    // Todo: Either first or Default should be used or replace where clause
                    var reserveQty = getRequisition.RequisitionLines.Where(i => i.ItemId == line.ItemId && i.WarehouseId == line.WarehouseId).Sum(i => i.ReserveQuantity);

                    //must be not checked on Rejection issuance
                    if (getState.State == DocumentStatus.Unpaid)
                    {
                        if (line.Quantity > reserveQty)
                            return new Response<bool>("Issuance quantity must not be greater than requested quantity");
                    }
                    if (getState.State == DocumentStatus.Unpaid)
                    {

                        // Deducting Reserve Quantity
                        var reqLine = getRequisition.RequisitionLines.Where(i => i.ItemId == line.ItemId && i.WarehouseId == line.WarehouseId).FirstOrDefault();
                        if (reqLine.Quantity > 0)
                        {
                            reqLine.setReserveQuantity(reqLine.ReserveQuantity - line.Quantity);
                        }
                    }

                    // updating reserved quantity for APPROVED Issuance
                    if (getState.State == DocumentStatus.Unpaid)
                    {
                        getStockRecord.updateRequisitionReservedQuantity(getStockRecord.ReservedRequisitionQuantity - line.Quantity);

                    }

                    // updating reserved quantity for REJECTED Issuance
                    //if (getState.State == DocumentStatus.Rejected)
                    //{
                    //    getStockRecord.updateRequisitionReservedQuantity(getStockRecord.ReservedRequisitionQuantity - line.Quantity);
                    //    getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity + line.Quantity);
                    //}

                }
            }
            return new Response<bool>(true, "");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Response<bool> CheckValidation(int requisitionId, RequisitionLines requisitionLine, IssuanceLines issuanceLine)
        {
            // Checking if given amount is greater than unreconciled document amount
            var reconciledRequistionQty = _unitOfWork.RequisitionToIssuanceLineReconcile
                .Find(new RequisitionToIssuanceLineReconcileSpecs(requisitionId, requisitionLine.Id, requisitionLine.ItemId))
                .Sum(p => p.Quantity);
            var unreconciledRequisitionQty = requisitionLine.Quantity - reconciledRequistionQty;
            if (issuanceLine.Quantity > unreconciledRequisitionQty)
                return new Response<bool>("Enter quantity is greater than pending quantity");

            return new Response<bool>(true, "No validation error found");
        }

        public async Task<Response<bool>> ReconcileReqLines(int issuanceId, int requisitionId, List<IssuanceLines> IssuanceLines)
        {
            foreach (var IssuanceLine in IssuanceLines)
            {
                //Getting Unreconciled Requisition lines
                var getRequisitionLine = _unitOfWork.Requisition
                    .FindLines(new RequisitionLinesSpecs(IssuanceLine.ItemId, requisitionId, IssuanceLine.WarehouseId ,true))
                    .FirstOrDefault();
                if (getRequisitionLine == null)
                    return new Response<bool>("No Requisition line found for reconciliaiton");

                var checkValidation = CheckValidation(requisitionId, getRequisitionLine, IssuanceLine);
                if (!checkValidation.IsSuccess)
                    return new Response<bool>(checkValidation.Message);

                //Adding in Reconcilation table
                var recons = new RequisitionToIssuanceLineReconcile(IssuanceLine.ItemId, IssuanceLine.Quantity,
                    requisitionId, issuanceId, getRequisitionLine.Id, IssuanceLine.Id, IssuanceLine.WarehouseId);
                await _unitOfWork.RequisitionToIssuanceLineReconcile.Add(recons);
                await _unitOfWork.SaveAsync();

                //Get total recon quantity
                var reconciledTotalReqQty = _unitOfWork.RequisitionToIssuanceLineReconcile
                    .Find(new RequisitionToIssuanceLineReconcileSpecs(requisitionId, getRequisitionLine.Id, getRequisitionLine.ItemId))
                    .Sum(p => p.Quantity);

                // Updationg Requisition line status
                if (getRequisitionLine.Quantity == reconciledTotalReqQty)
                {
                    getRequisitionLine.setStatus(DocumentStatus.Reconciled);
                }
                else
                {
                    getRequisitionLine.setStatus(DocumentStatus.Partial);
                }
                await _unitOfWork.SaveAsync();
            }

            //Update Requisition Master Status
            var getrequisition = await _unitOfWork.Requisition
                    .GetById(requisitionId, new RequisitionSpecs());

            var isRequisitionLinesReconciled = getrequisition.RequisitionLines
                .Where(x => x.Status == DocumentStatus.Unreconciled || x.Status == DocumentStatus.Partial)
                .FirstOrDefault();

            if (isRequisitionLinesReconciled == null)
            {
                getrequisition.setStatus(5);
            }
            else
            {
                getrequisition.setStatus(4);
            }

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "No validation error found");
        }

        private IssuanceDto MapToValue(IssuanceDto data)
        {
            //Get reconciled grns
            var grnLineReconcileRecord = _unitOfWork.IssuanceToIssuanceReturnLineReconcile
                .Find(new IssuanceToIssuanceReturnLineReconcileSpecs(true, data.Id))
                .GroupBy(x => new { x.IssuanceReturnId, x.IssuanceReturn.DocNo })
                .Where(g => g.Count() >= 1)
                .Select(y => new
                {
                    GRNId = y.Key.IssuanceReturnId,
                    DocNo = y.Key.DocNo,
                })
                .ToList();

            // Adding in grns in references list
            var getReference = new List<ReferncesDto>();
            if (grnLineReconcileRecord.Any())
            {
                foreach (var line in grnLineReconcileRecord)
                {
                    getReference.Add(new ReferncesDto
                    {
                        DocId = line.GRNId,
                        DocNo = line.DocNo,
                        DocType = DocType.GRN
                    });
                }
            }
            data.References = getReference;

            // Get pending & received quantity...
            foreach (var line in data.IssuanceLines)
            {
                // Checking if given amount is greater than unreconciled document amount
                line.ReceivedQuantity = _unitOfWork.IssuanceToIssuanceReturnLineReconcile
                    .Find(new IssuanceToIssuanceReturnLineReconcileSpecs(data.Id, line.Id, line.ItemId, line.WarehouseId))
                    .Sum(p => p.Quantity);

                line.PendingQuantity = line.Quantity - line.ReceivedQuantity;
            }

            return data;
        }
        private List<RemarksDto> ReturningRemarks(IssuanceDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.Issuance))
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
