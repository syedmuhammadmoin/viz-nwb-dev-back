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
    public class IssuanceReturnService : IIssuanceReturnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IssuanceReturnService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getIssuanceReturn = await _unitOfWork.IssuanceReturn.GetById(data.DocId, new IssuanceReturnSpecs(true));

            if (getIssuanceReturn == null)
            {
                return new Response<bool>("IssuanceReturn with the input id not found");
            }
            if (getIssuanceReturn.Status.State == DocumentStatus.Unpaid || getIssuanceReturn.Status.State == DocumentStatus.Partial || getIssuanceReturn.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("IssuanceReturn already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.IssuanceReturn)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getIssuanceReturn.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
        
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getIssuanceReturn.setStatus(transition.NextStatusId);
                        if (!String.IsNullOrEmpty(data.Remarks))
                        {
                            var addRemarks = new Remark()
                            {
                                DocId = getIssuanceReturn.Id,
                                DocType = DocType.IssuanceReturn,
                                Remarks = data.Remarks,
                                UserId = userId
                            };
                            await _unitOfWork.Remarks.Add(addRemarks);
                        }
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            foreach (var line in getIssuanceReturn.IssuanceReturnLines)
                            {
                                line.setStatus(DocumentStatus.Unreconciled);
                            }

                            var reconciled = await ReconcileIssuanceLines(getIssuanceReturn.Id, (int)getIssuanceReturn.IssuanceId, getIssuanceReturn.IssuanceReturnLines);
                            if (!reconciled.IsSuccess)
                            {
                                _unitOfWork.Rollback();
                                return new Response<bool>(reconciled.Message);
                            }

                            await _unitOfWork.SaveAsync();

                            //Adding IssuanceReturn Line in Stock
                            await AddandUpdateStock(getIssuanceReturn);


                            _unitOfWork.Commit();
                            return new Response<bool>(true, "IssuanceReturn Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "IssuanceReturn Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "IssuanceReturn Reviewed");
                    }
                }

                return new Response<bool>("User does not have allowed role");

          
        }

        public async Task<Response<IssuanceReturnDto>> CreateAsync(CreateIssuanceReturnDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitIssuanceReturn(entity);
            }
            else
            {
                return await this.SaveIssuanceReturn(entity, 1);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<IssuanceReturnDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var issuanceReturn = await _unitOfWork.IssuanceReturn.GetAll(new IssuanceReturnSpecs(docDate, states, filter, false));

            if (issuanceReturn.Count() == 0)
                return new PaginationResponse<List<IssuanceReturnDto>>(_mapper.Map<List<IssuanceReturnDto>>(issuanceReturn), "List is empty");

            var totalRecords = await _unitOfWork.IssuanceReturn.TotalRecord(new IssuanceReturnSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<IssuanceReturnDto>>(_mapper.Map<List<IssuanceReturnDto>>(issuanceReturn),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<IssuanceReturnDto>> GetByIdAsync(int id)
        {

            var specification = new IssuanceReturnSpecs(false);
            var issuanceReturn = await _unitOfWork.IssuanceReturn.GetById(id, specification);
            if (issuanceReturn == null)
                return new Response<IssuanceReturnDto>("Not found");

            var issuanceReturnDto = _mapper.Map<IssuanceReturnDto>(issuanceReturn);
            ReturningRemarks(issuanceReturnDto, DocType.IssuanceReturn);
            issuanceReturnDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.IssuanceReturn)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == issuanceReturnDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            issuanceReturnDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<IssuanceReturnDto>(issuanceReturnDto, "Returning value");

        }

        public async Task<Response<IssuanceReturnDto>> UpdateAsync(CreateIssuanceReturnDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitIssuanceReturn(entity);
            }
            else
            {
                return await this.UpdateIssuanceReturn(entity, 1);
            }
        }

        //Privte Methods for IssuanceReturn

        private async Task<Response<IssuanceReturnDto>> SubmitIssuanceReturn(CreateIssuanceReturnDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.IssuanceReturn)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<IssuanceReturnDto>("No workflow found for IssuanceReturn");
            }

            if (entity.Id == null)
            {
                return await this.SaveIssuanceReturn(entity, 6);
            }
            else
            {
                return await this.UpdateIssuanceReturn(entity, 6);
            }
        }

        private async Task<Response<IssuanceReturnDto>> SaveIssuanceReturn(CreateIssuanceReturnDto entity, int status)
        {
            if (entity.IssuanceReturnLines.Count() == 0)
                return new Response<IssuanceReturnDto>("Lines are required");

            foreach (var issuanceReturnLine in entity.IssuanceReturnLines)
            {
                //Getting Unreconciled IssuanceReturn lines
                var getIssuanceLine = _unitOfWork.Issuance
                    .FindLines(new IssuanceLinesSpecs((int)issuanceReturnLine.ItemId, (int)issuanceReturnLine.WarehouseId, (int)entity.IssuanceId))
                    .FirstOrDefault();
                if (getIssuanceLine == null)
                    return new Response<IssuanceReturnDto>("No IssuanceReturn line found for reconciliaiton");

                var checkValidation = CheckValidation((int)entity.IssuanceId, getIssuanceLine, _mapper.Map<IssuanceReturnLines>(issuanceReturnLine));
                if (!checkValidation.IsSuccess)
                    return new Response<IssuanceReturnDto>(checkValidation.Message);
            }

            //Checking duplicate Lines if any
            var duplicates = entity.IssuanceReturnLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<IssuanceReturnDto>("Duplicate Lines found");

            var issuanceReturn = _mapper.Map<IssuanceReturnMaster>(entity);

            //Setting status
            issuanceReturn.setStatus(status);

            _unitOfWork.CreateTransaction();
          
                //Saving in table
                var result = await _unitOfWork.IssuanceReturn.Add(issuanceReturn);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                issuanceReturn.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<IssuanceReturnDto>(_mapper.Map<IssuanceReturnDto>(result), "Created successfully");
           
        }

        private async Task<Response<IssuanceReturnDto>> UpdateIssuanceReturn(CreateIssuanceReturnDto entity, int status)
        {
            //setting IssuanceReturnId
            var getIssuance = await _unitOfWork.Issuance.GetById((int)entity.IssuanceId, new IssuanceSpecs(false));

            if (getIssuance == null)
                return new Response<IssuanceReturnDto>("Issuance not found");

            if (entity.IssuanceReturnLines.Count() == 0)
                return new Response<IssuanceReturnDto>("Lines are required");

            foreach (var issuanceReturnLine in entity.IssuanceReturnLines)
            {
                //Getting Unreconciled IssuanceReturn lines
                var getIssuanceLine = _unitOfWork.Issuance
                    .FindLines(new IssuanceLinesSpecs((int)issuanceReturnLine.ItemId, (int)issuanceReturnLine.WarehouseId, (int)entity.IssuanceId))
                    .FirstOrDefault();
                if (getIssuanceLine == null)
                    return new Response<IssuanceReturnDto>("No IssuanceReturn line found for reconciliaiton");

                var checkValidation = CheckValidation((int)entity.IssuanceId, getIssuanceLine, _mapper.Map<IssuanceReturnLines>(issuanceReturnLine));
                if (!checkValidation.IsSuccess)
                    return new Response<IssuanceReturnDto>(checkValidation.Message);
            }

            //Checking duplicate Lines if any
            var duplicates = entity.IssuanceReturnLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<IssuanceReturnDto>("Duplicate Lines found");

            var specification = new IssuanceReturnSpecs(true);
            var gRN = await _unitOfWork.IssuanceReturn.GetById((int)entity.Id, specification);

            if (gRN == null)
                return new Response<IssuanceReturnDto>("Not found");

            if (gRN.StatusId != 1 && gRN.StatusId != 2)
                return new Response<IssuanceReturnDto>("Only draft document can be edited");
            //Setting IssuanceReturnId
            gRN.setIssuanceId(getIssuance.Id);

            //Setting status
            gRN.setStatus(status);

            _unitOfWork.CreateTransaction();
         
                //For updating data
                _mapper.Map<CreateIssuanceReturnDto, IssuanceReturnMaster>(entity, gRN);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<IssuanceReturnDto>(_mapper.Map<IssuanceReturnDto>(gRN), "Created successfully");
          
        }

        public Response<bool> CheckValidation(int issuanceId, IssuanceLines issuanceLine, IssuanceReturnLines issuanceReturnLine)
        {
            // Checking if given amount is greater than unreconciled document amount
            var reconciledIssuanceQty = _unitOfWork.IssuanceToIssuanceReturnLineReconcile
                .Find(new IssuanceToIssuanceReturnLineReconcileSpecs(issuanceId, issuanceLine.Id, issuanceLine.ItemId, issuanceLine.WarehouseId))
                .Sum(p => p.Quantity);

            var unreconciledIssuanceQty = issuanceLine.Quantity - reconciledIssuanceQty;

            if (issuanceReturnLine.Quantity > unreconciledIssuanceQty)
                return new Response<bool>("Enter quantity is greater than return quantity");

            return new Response<bool>(true, "No validation error found");
        }

        public async Task AddandUpdateStock(IssuanceReturnMaster issuanceReturn)
        {
            foreach (var line in issuanceReturn.IssuanceReturnLines)
            {
                var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs(line.ItemId, line.WarehouseId)).FirstOrDefault();

                if (getStockRecord == null)
                {
                    var addStock = new Stock(
                        line.ItemId,
                        line.Quantity,
                        0,
                        line.WarehouseId
                    );

                    await _unitOfWork.Stock.Add(addStock);
                }
                else
                {
                    getStockRecord.updateAvailableQuantity(getStockRecord.AvailableQuantity + line.Quantity);
                }

                await _unitOfWork.SaveAsync();

            }
        }

        public async Task<Response<bool>> ReconcileIssuanceLines(int issuanceReturnId, int issuanceId, List<IssuanceReturnLines> issuanceReturnLines)
        {
            foreach (var issuanceReturnLine in issuanceReturnLines)
            {
                //Getting Unreconciled Purchase Order lines
                var getIssuanceLine = _unitOfWork.Issuance
                    .FindLines(new IssuanceLinesSpecs(issuanceReturnLine.ItemId, issuanceReturnLine.WarehouseId, issuanceId))
                    .FirstOrDefault();
                if (getIssuanceLine == null)
                    return new Response<bool>("No Purchase order line found for reconciliaiton");

                var checkValidation = CheckValidation(issuanceId, getIssuanceLine, issuanceReturnLine);
                if (!checkValidation.IsSuccess)
                    return new Response<bool>(checkValidation.Message);

                //Adding in Reconcilation table
                var recons = new IssuanceToIssuanceReturnLineReconcile(issuanceReturnLine.ItemId, issuanceReturnLine.Quantity,
                    issuanceId, issuanceReturnId, getIssuanceLine.Id, issuanceReturnLine.Id, issuanceReturnLine.WarehouseId);
                await _unitOfWork.IssuanceToIssuanceReturnLineReconcile.Add(recons);
                await _unitOfWork.SaveAsync();

                //Get total recon quantity
                var reconciledTotalPOQty = _unitOfWork.IssuanceToIssuanceReturnLineReconcile
                    .Find(new IssuanceToIssuanceReturnLineReconcileSpecs(issuanceId, getIssuanceLine.Id, getIssuanceLine.ItemId, getIssuanceLine.WarehouseId))
                    .Sum(p => p.Quantity);

                // Updationg PO line status
                if (getIssuanceLine.Quantity == reconciledTotalPOQty)
                {
                    getIssuanceLine.setStatus(DocumentStatus.Reconciled);
                }
                else
                {
                    getIssuanceLine.setStatus(DocumentStatus.Partial);
                }
                await _unitOfWork.SaveAsync();
            }

            //Update Issuance Status
            var getissuance = await _unitOfWork.Issuance
                    .GetById(issuanceId, new IssuanceSpecs());

            var isIssuanceLinesReconciled = getissuance.IssuanceLines
                .Where(x => x.Status == DocumentStatus.Unreconciled || x.Status == DocumentStatus.Partial)
                .FirstOrDefault();

            if (isIssuanceLinesReconciled == null)
            {
                getissuance.setStatus(5);
            }
            else
            {
                getissuance.setStatus(4);
            }

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "No validation error found");
        }
        private List<RemarksDto> ReturningRemarks(IssuanceReturnDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.IssuanceReturn))
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
