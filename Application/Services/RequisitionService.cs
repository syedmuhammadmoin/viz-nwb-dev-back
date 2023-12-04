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

            var requisitionDto = _mapper.Map<RequisitionDto>(requisition);

            CheckingConditions(requisitionDto, requisitionProductItemIds);

            if ((requisitionDto.State == DocumentStatus.Unpaid || requisitionDto.State == DocumentStatus.Partial || requisitionDto.State == DocumentStatus.Paid))
                return new Response<RequisitionDto>(MapToValue(requisitionDto), "Returning value");

            ReturningRemarks(requisitionDto);
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
                return await this.UpdateRequisition(entity, 1);
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
                    getRequisition.SetStatus(transition.NextStatusId);
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
                            line.SetStatus(DocumentStatus.Unreconciled);


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
                            if (getStockRecord != null)
                            {
                                getStockRecord.UpdateRequisitionReservedQuantity(getStockRecord.ReservedRequisitionQuantity - line.ReserveQuantity);
                                getStockRecord.UpdateAvailableQuantity(getStockRecord.AvailableQuantity + line.ReserveQuantity);
                                line.SetReserveQuantity(0);
                            }
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
            var requisition = CheckOrUpdateQty(entity).Result;

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

        private async Task<CreateRequisitionDto> CheckOrUpdateQty(CreateRequisitionDto entity)
        {
            foreach (var line in entity.RequisitionLines)
            {
                // non fixed asset
                if (line.FixedAssetId == null)
                {
                    var getStockRecord = _unitOfWork.Stock.Find(new StockSpecs((int)line.ItemId, (int)line.WarehouseId)).FirstOrDefault();
                    if (getStockRecord != null)
                    {
                        if (getStockRecord.AvailableQuantity > 0)
                        {
                            int reserveableQuantity = (int)line.Quantity;

                            if (line.Quantity > getStockRecord.AvailableQuantity)
                                reserveableQuantity = getStockRecord.AvailableQuantity;

                            //Need to Save reserveable Quantity with Requesition
                            line.ReserveQuantity = reserveableQuantity;
                            getStockRecord.UpdateRequisitionReservedQuantity(getStockRecord.ReservedRequisitionQuantity + reserveableQuantity);
                            getStockRecord.UpdateAvailableQuantity(getStockRecord.AvailableQuantity - reserveableQuantity);
                        }
                    }
                }
                // Fixed Asset
                else
                {
                    var fixedAsset = await _unitOfWork.FixedAsset.GetById(line.FixedAssetId.Value);
                    if (fixedAsset.IsReserved)
                    {
                        // no reserve allow
                    }
                    else
                    {
                    fixedAsset.SetIsReserved(true);
                    }

                }
            }
            return entity;
        }

        private async Task<Response<RequisitionDto>> SaveRequisition(CreateRequisitionDto entity, int status)
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


            foreach (var requisitionLines in entity.RequisitionLines)
            {
                if (requisitionLines.FixedAssetId != null)
                {
                    const int singleUnitOfAsset = 1;
                    requisitionLines.Quantity = singleUnitOfAsset;
                }
            }


            var requisition = _mapper.Map<RequisitionMaster>(entity);

            //Setting status
            requisition.SetStatus(status);

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

            foreach (var requisitionLines in entity.RequisitionLines)
            {
                if (requisitionLines.FixedAssetId != null)
                {
                    const int singleUnitOfAsset = 1;
                    requisitionLines.Quantity = singleUnitOfAsset;
                }
            }


            //Setting status
            requisition.SetStatus(status);

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
            List<ReferncesDto> references = new List<ReferncesDto>();
            //Add Reference
            if (data.RequestId != null)
            {

                references.Add(new ReferncesDto()
                {
                    DocId = (int)data.RequestId,
                    DocNo = "REQUEST-" + String.Format("{0:000}", data.RequestId),
                    DocType = DocType.Request
                });

                data.References = references;
            }
            var purchaseOrder = _unitOfWork.PurchaseOrder.Find(new PurchaseOrderSpecs(data.Id, true));
            if (purchaseOrder != null)
            {
                foreach (var po in purchaseOrder)
                {
                    references.Add(new
                        ReferncesDto()
                    {
                        DocId = po.Id,
                        DocNo = po.DocNo,
                        DocType = DocType.PurchaseOrder
                    });

                }
            }
            var quotations = _unitOfWork.Quotation.Find(new QuotationSpecs(data.Id, true));

            if (quotations != null)
            {
                foreach (var quote in quotations)
                {
                    references.Add(
                        new ReferncesDto()
                        {
                            DocId = quote.Id,
                            DocNo = quote.DocNo,
                            DocType = DocType.Quotation
                        }
                     );
                }
                data.References = references;
            }
            // Adding in issuances in references list
            if (grnLineReconcileRecord.Any())
            {

                foreach (var line in grnLineReconcileRecord)
                {
                    references.Add(new ReferncesDto
                    {
                        DocId = line.IssuanceId,
                        DocNo = line.DocNo,
                        DocType = DocType.Issuance
                    });
                }
            }
            data.References = references;

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

        private RequisitionDto CheckingConditions(RequisitionDto requisitionDto, List<int> requisitionProductItemIds)
        {
            var stock = _unitOfWork.Stock.GetAll(new StockSpecs(requisitionProductItemIds)).Result;

            bool isRequiredQty = false;
            List<decimal> purchaseAmounts = new List<decimal>();

            if (requisitionDto.State != DocumentStatus.Paid)
            {
                foreach (var line in requisitionDto.RequisitionLines)
                {
                    if (stock.Count() > 0)
                    {
                        line.AvailableQuantity = stock.Where(x => x.ItemId == line.ItemId && x.WarehouseId == line.WarehouseId).Select(i => i.AvailableQuantity).FirstOrDefault();
                    }

                    if (line.ReserveQuantity > 0)
                    {
                        requisitionDto.IsShowIssuanceButton = true;
                    }

                    if (line.Quantity > line.ReserveQuantity)
                    {
                        var IssuedQuantity = _unitOfWork.RequisitionToIssuanceLineReconcile
                                                  .Find(new RequisitionToIssuanceLineReconcileSpecs(line.MasterId,
                                                  line.Id, line.ItemId)).Sum(p => p.Quantity);
                        var requiredQuantity = line.Quantity - (line.ReserveQuantity + IssuedQuantity);
                        if (line.Quantity > (line.ReserveQuantity + IssuedQuantity))
                        {
                            isRequiredQty = true;

                        }

                        purchaseAmounts.Add(requiredQuantity * line.PurchasePrice);
                    }
                }

                var totalAmount = purchaseAmounts.Sum();
                if (isRequiredQty)
                {
                    if (totalAmount <= 100000)
                    {
                        requisitionDto.IsShowPurchaseOrderButton = true;
                    }
                    else if (totalAmount > 100000 && totalAmount <= 300000)
                    {
                        requisitionDto.IsShowCFQButton = true;
                        requisitionDto.IsShowQuotationButton = true;
                    }
                    else if (totalAmount > 300000)
                    {
                        requisitionDto.IsShowTenderButton = true;
                    }

                }
            }
            return requisitionDto;

        }

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
