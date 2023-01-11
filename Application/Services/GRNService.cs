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
    public class GRNService : IGRNService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GRNService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getGRN = await _unitOfWork.GRN.GetById(data.DocId, new GRNSpecs(true));

            if (getGRN == null)
            {
                return new Response<bool>("GRN with the input id not found");
            }
            if (getGRN.Status.State == DocumentStatus.Unpaid || getGRN.Status.State == DocumentStatus.Partial || getGRN.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("GRN already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GRN)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getGRN.StatusId && x.Action == data.Action));

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
                    getGRN.setStatus(transition.NextStatusId);
                    if (!String.IsNullOrEmpty(data.Remarks))
                    {
                        var addRemarks = new Remark()
                        {
                            DocId = getGRN.Id,
                            DocType = DocType.GRN,
                            Remarks = data.Remarks,
                            UserId = userId
                        };
                        await _unitOfWork.Remarks.Add(addRemarks);
                    }
                    bool isRequisition = false;
                    if (transition.NextStatus.State == DocumentStatus.Unpaid)
                    {
                        foreach (var grnline in getGRN.GRNLines)
                        {
                            grnline.setStatus(DocumentStatus.Unreconciled);

                            var purchseOrder = await _unitOfWork.PurchaseOrder.GetById(getGRN.PurchaseOrderId, new PurchaseOrderSpecs((int)getGRN.PurchaseOrderId));
                            if (purchseOrder != null)
                            {
                                if (purchseOrder.RequisitionId != null)
                                {
                                    var requisition = await _unitOfWork.Requisition.GetById((int)purchseOrder.RequisitionId, new RequisitionSpecs((int)purchseOrder.RequisitionId));
                                    if (requisition != null)
                                    {
                                        isRequisition = true;
                                        var reqLine = _unitOfWork.Requisition.FindLines(new RequisitionLinesSpecs(grnline.ItemId, grnline.WarehouseId, (int)purchseOrder.RequisitionId)).FirstOrDefault();
                                        var IssuedQuantity = _unitOfWork.RequisitionToIssuanceLineReconcile
                                                    .Find(new RequisitionToIssuanceLineReconcileSpecs(reqLine.MasterId,
                                                    reqLine.Id, reqLine.ItemId)).Sum(p => p.Quantity);
                                        var reqRemainingQty = reqLine.Quantity - reqLine.ReserveQuantity - IssuedQuantity;
                                        var stock = _unitOfWork.Stock
                                                   .Find(new StockSpecs(reqLine.ItemId, reqLine.WarehouseId))
                                                   .FirstOrDefault();
                                        if (grnline.Quantity <= reqRemainingQty)
                                        {
                                            reqLine.setReserveQuantity(reqLine.ReserveQuantity + grnline.Quantity);
                                            stock.updateRequisitionReservedQuantity(stock.ReservedRequisitionQuantity + grnline.Quantity);
                                        }
                                        else
                                        {
                                            reqLine.setReserveQuantity(reqLine.ReserveQuantity + reqRemainingQty);
                                            stock.updateRequisitionReservedQuantity(stock.ReservedRequisitionQuantity + reqRemainingQty);
                                            stock.updateAvailableQuantity(grnline.Quantity - reqRemainingQty);

                                        }

                                    }
                                }
                            }
                        }

                        var reconciled = await ReconcilePOLines(getGRN.Id, (int)getGRN.PurchaseOrderId, getGRN.GRNLines);
                        if (!reconciled.IsSuccess)
                        {
                            _unitOfWork.Rollback();
                            return new Response<bool>(reconciled.Message);
                        }

                        await _unitOfWork.SaveAsync();

                        //Adding GRN Line in Stock
                        if (isRequisition == false)
                        {
                            await AddandUpdateStock(getGRN);
                        }

                        _unitOfWork.Commit();
                        return new Response<bool>(true, "GRN Approved");
                    }
                    if (transition.NextStatus.State == DocumentStatus.Rejected)
                    {
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "GRN Rejected");
                    }
                    await _unitOfWork.SaveAsync();
                    _unitOfWork.Commit();
                    return new Response<bool>(true, "GRN Reviewed");
                }
            }

            return new Response<bool>("User does not have allowed role");


        }

        public async Task<Response<GRNDto>> CreateAsync(CreateGRNDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitGRN(entity);
            }
            else
            {
                return await this.SaveGRN(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<GRNDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var gRN = await _unitOfWork.GRN.GetAll(new GRNSpecs(docDate, states, filter, false));

            if (gRN.Count() == 0)
                return new PaginationResponse<List<GRNDto>>(_mapper.Map<List<GRNDto>>(gRN), "List is empty");

            var totalRecords = await _unitOfWork.GRN.TotalRecord(new GRNSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<GRNDto>>(_mapper.Map<List<GRNDto>>(gRN),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<GRNDto>> GetByIdAsync(int id)
        {
            var specification = new GRNSpecs(false);
            var gRN = await _unitOfWork.GRN.GetById(id, specification);
            if (gRN == null)
                return new Response<GRNDto>("Not found");

            var grnDto = _mapper.Map<GRNDto>(gRN);
            ReturningRemarks(grnDto, DocType.GRN);
            ReturningFiles(grnDto, DocType.GRN);
            var mappingValue = new Response<GRNDto>(MapToValue(grnDto), "Returning value");

            grnDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GRN)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == grnDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            grnDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<GRNDto>(grnDto, "Returning value");

        }

        public async Task<Response<GRNDto>> UpdateAsync(CreateGRNDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitGRN(entity);
            }
            else
            {
                return await this.UpdateGRN(entity, 1);
            }
        }

        //Privte Methods for GRN

        private async Task<Response<GRNDto>> SubmitGRN(CreateGRNDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GRN)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<GRNDto>("No workflow found for GRN");
            }

            if (entity.Id == null)
            {
                return await this.SaveGRN(entity, 6);
            }
            else
            {
                return await this.UpdateGRN(entity, 6);
            }
        }

        private async Task<Response<GRNDto>> SaveGRN(CreateGRNDto entity, int status)
        {
            if (entity.GRNLines.Count() == 0)
                return new Response<GRNDto>("Lines are required");

            foreach (var grnLine in entity.GRNLines)
            {
                //Getting Unreconciled Purchase Order lines
                var getpurchaseOrderLine = _unitOfWork.PurchaseOrder
                .FindLines(new PurchaseOrderLinesSpecs((int)grnLine.ItemId, (int)grnLine.WarehouseId, (int)entity.PurchaseOrderId))
                .FirstOrDefault();
                if (getpurchaseOrderLine == null)
                    return new Response<GRNDto>("No Purchase order line found for reconciliaiton");

                var checkValidation = CheckValidationForPO((int)entity.PurchaseOrderId, getpurchaseOrderLine, _mapper.Map<GRNLines>(grnLine));
                if (!checkValidation.IsSuccess)
                    return new Response<GRNDto>(checkValidation.Message);

            }

            //Checking duplicate Lines if any
            var duplicates = entity.GRNLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<GRNDto>("Duplicate Lines found");

            var grn = _mapper.Map<GRNMaster>(entity);

            //Setting status
            grn.setStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.GRN.Add(grn);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            grn.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<GRNDto>(_mapper.Map<GRNDto>(result), "Created successfully");

        }

        private async Task<Response<GRNDto>> UpdateGRN(CreateGRNDto entity, int status)
        {
            //setting PurchaseOrderId
            var getPO = await _unitOfWork.PurchaseOrder.GetById((int)entity.PurchaseOrderId, new PurchaseOrderSpecs(false));

            if (getPO == null)
                return new Response<GRNDto>("Purchase Order not found");

            if (entity.GRNLines.Count() == 0)
                return new Response<GRNDto>("Lines are required");

            foreach (var grnLine in entity.GRNLines)
            {

                var getpurchaseOrderLine = _unitOfWork.PurchaseOrder
                    .FindLines(new PurchaseOrderLinesSpecs((int)grnLine.ItemId, (int)grnLine.WarehouseId, (int)entity.PurchaseOrderId))
                    .FirstOrDefault();
                if (getpurchaseOrderLine == null)
                    return new Response<GRNDto>("No Purchase order line found for reconciliaiton");

                var checkValidation = CheckValidationForPO((int)entity.PurchaseOrderId, getpurchaseOrderLine, _mapper.Map<GRNLines>(grnLine));
                if (!checkValidation.IsSuccess)
                    return new Response<GRNDto>(checkValidation.Message);

            }

            //Checking duplicate Lines if any
            var duplicates = entity.GRNLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<GRNDto>("Duplicate Lines found");

            var specification = new GRNSpecs(true);
            var gRN = await _unitOfWork.GRN.GetById((int)entity.Id, specification);

            if (gRN == null)
                return new Response<GRNDto>("Not found");

            if (gRN.StatusId != 1 && gRN.StatusId != 2)
                return new Response<GRNDto>("Only draft document can be edited");
            //Setting PurchaseId
            gRN.setPurchaseOrderId(getPO.Id);

            //Setting status
            gRN.setStatus(status);

            _unitOfWork.CreateTransaction();
            //For updating data
            _mapper.Map<CreateGRNDto, GRNMaster>(entity, gRN);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<GRNDto>(_mapper.Map<GRNDto>(gRN), "Updated successfully");

        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> ReconcilePOLines(int grnId, int purchaseOrderId, List<GRNLines> grnLines)
        {
            foreach (var grnLine in grnLines)
            {
                //Getting Unreconciled Purchase Order lines
                var getpurchaseOrderLine = _unitOfWork.PurchaseOrder
                    .FindLines(new PurchaseOrderLinesSpecs(grnLine.ItemId, grnLine.WarehouseId, purchaseOrderId))
                    .FirstOrDefault();
                if (getpurchaseOrderLine == null)
                    return new Response<bool>("No Purchase order line found for reconciliaiton");

                var checkValidation = CheckValidationForPO(purchaseOrderId, getpurchaseOrderLine, grnLine);
                if (!checkValidation.IsSuccess)
                    return new Response<bool>(checkValidation.Message);

                //Adding in Reconcilation table
                var recons = new POToGRNLineReconcile(grnLine.ItemId, grnLine.Quantity,
                    purchaseOrderId, grnId, getpurchaseOrderLine.Id, grnLine.Id, grnLine.WarehouseId);
                await _unitOfWork.POToGRNLineReconcile.Add(recons);
                await _unitOfWork.SaveAsync();

                //Get total recon quantity
                var reconciledTotalPOQty = _unitOfWork.POToGRNLineReconcile
                    .Find(new POToGRNLineReconcileSpecs(purchaseOrderId, getpurchaseOrderLine.Id, getpurchaseOrderLine.ItemId, getpurchaseOrderLine.WarehouseId))
                    .Sum(p => p.Quantity);

                // Updationg PO line status
                if (getpurchaseOrderLine.Quantity == reconciledTotalPOQty)
                {
                    getpurchaseOrderLine.setStatus(DocumentStatus.Reconciled);
                }
                else
                {
                    getpurchaseOrderLine.setStatus(DocumentStatus.Partial);
                }
                await _unitOfWork.SaveAsync();
            }

            //Update Purchase Order Master Status
            var getpurchaseOrder = await _unitOfWork.PurchaseOrder
                    .GetById(purchaseOrderId, new PurchaseOrderSpecs());

            var isPOLinesReconciled = getpurchaseOrder.PurchaseOrderLines
                .Where(x => x.Status == DocumentStatus.Unreconciled || x.Status == DocumentStatus.Partial)
                .FirstOrDefault();

            if (isPOLinesReconciled == null)
            {
                getpurchaseOrder.SetStatus(5);
            }
            else
            {
                getpurchaseOrder.SetStatus(4);
            }

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "No validation error found");
        }

        public async Task AddandUpdateStock(GRNMaster grn)
        {
            foreach (var line in grn.GRNLines)
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

        public Response<bool> CheckValidationForPO(int purchaseOrderId, PurchaseOrderLines purchaseOrderLine, GRNLines grnLine)
        {
            // Checking if given amount is greater than unreconciled document amount
            var reconciledPOQty = _unitOfWork.POToGRNLineReconcile
                .Find(new POToGRNLineReconcileSpecs(purchaseOrderId, purchaseOrderLine.Id, purchaseOrderLine.ItemId, purchaseOrderLine.WarehouseId))
                .Sum(p => p.Quantity);
            var unreconciledPOQty = purchaseOrderLine.Quantity - reconciledPOQty;
            if (grnLine.Quantity > unreconciledPOQty)
                return new Response<bool>("Enter quantity is greater than pending quantity");

            return new Response<bool>(true, "No validation error found");
        }

        private GRNDto MapToValue(GRNDto data)
        {

            var getReference = new List<ReferncesDto>();

            if ((data.State == DocumentStatus.Partial || data.State == DocumentStatus.Paid))
            {
                //Get reconciled goodsReturnNote
                var goodsReturnNoteReconcileRecord = _unitOfWork.GRNToGoodsReturnNoteLineReconcile
                    .Find(new GRNToGoodsReturnNoteLineReconcileSpecs(data.Id))
                    .GroupBy(x => new { x.GoodsReturnNoteId, x.GoodsReturnNote.DocNo })
                    .Where(g => g.Count() >= 1)
                    .Select(y => new
                    {
                        GoodsReturnNoteId = y.Key.GoodsReturnNoteId,
                        DocNo = y.Key.DocNo,
                    })
                    .ToList();

                if (goodsReturnNoteReconcileRecord.Any())
                {
                    foreach (var line in goodsReturnNoteReconcileRecord)
                    {
                        getReference.Add(new ReferncesDto
                        {
                            DocId = line.GoodsReturnNoteId,
                            DocNo = line.DocNo,
                            DocType = DocType.GoodsReturnNote,
                        });
                    }
                }
            }


            //Get bill reference in GRN
            var getBillForGRNReference = _unitOfWork.Bill
               .Find(new BillSpecs(data.Id, true)).FirstOrDefault();

            // Adding in Bill reference in GRN 

            if (getBillForGRNReference != null)
            {
                data.BillReference = (new ReferncesDto
                {
                    DocId = getBillForGRNReference.Id,
                    DocNo = getBillForGRNReference.DocNo,
                    DocType = DocType.Bill,
                });
            }

            data.References = getReference;

            // Get pending & received quantity...
            foreach (var line in data.GRNLines)
            {
                // Checking if given amount is greater than unreconciled document amount
                line.ReceivedQuantity = _unitOfWork.GRNToGoodsReturnNoteLineReconcile
                    .Find(new GRNToGoodsReturnNoteLineReconcileSpecs(data.Id, line.Id, line.ItemId, line.WarehouseId))
                    .Sum(p => p.Quantity);

                line.PendingQuantity = line.Quantity - line.ReceivedQuantity;
            }

            return data;
        }
        private List<RemarksDto> ReturningRemarks(GRNDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.GRN))
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
        private List<FileUploadDto> ReturningFiles(GRNDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.GRN))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.GRN,
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
