using Application.Contracts.DTOs;
using Application.Contracts.DTOs.FileUpload;
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
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPO(entity);
            }
            else
            {
                return await this.SavePO(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<PurchaseOrderDto>>> GetAllAsync(TransactionFormFilter filter)
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
            var po = await _unitOfWork.PurchaseOrder.GetAll(new PurchaseOrderSpecs(docDate, dueDate, states, filter, false));

            if (po.Count() == 0)
                return new PaginationResponse<List<PurchaseOrderDto>>(_mapper.Map<List<PurchaseOrderDto>>(po), "List is empty");

            var totalRecords = await _unitOfWork.PurchaseOrder.TotalRecord(new PurchaseOrderSpecs(docDate, dueDate, states, filter, true));

            return new PaginationResponse<List<PurchaseOrderDto>>(_mapper.Map<List<PurchaseOrderDto>>(po),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<PurchaseOrderDto>> GetByIdAsync(int id)
        {
            var specification = new PurchaseOrderSpecs(false);
            var po = await _unitOfWork.PurchaseOrder.GetById(id, specification);

            if (po == null)
                return new Response<PurchaseOrderDto>("Not found");

            var poDto = _mapper.Map<PurchaseOrderDto>(po);
            ReturningRemarks(poDto, DocType.PurchaseOrder);
            ReturningFiles(poDto, DocType.PurchaseOrder);
            if ((poDto.State == DocumentStatus.Partial || poDto.State == DocumentStatus.Paid))
            {
                return new Response<PurchaseOrderDto>(MapToValue(poDto), "Returning value");

            }

            poDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PurchaseOrder)).FirstOrDefault();
            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == poDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            poDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<PurchaseOrderDto>(poDto, "Returning value");
        }

        public async Task<Response<PurchaseOrderDto>> UpdateAsync(CreatePurchaseOrderDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitPO(entity);
            }
            else
            {
                return await this.UpdatePO(entity, 1);
            }
        }

        public async Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            var getPurchaseOrder = await _unitOfWork.PurchaseOrder.GetById(data.DocId, new PurchaseOrderSpecs(true));

            if (getPurchaseOrder == null)
            {
                return new Response<bool>("Purchase Order with the input id not found");
            }
            if (getPurchaseOrder.Status.State == DocumentStatus.Unpaid || getPurchaseOrder.Status.State == DocumentStatus.Partial || getPurchaseOrder.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Purchase Order already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PurchaseOrder)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getPurchaseOrder.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var getUser = new GetUser(this._httpContextAccessor);
            var userId = getUser.GetCurrentUserId();
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getPurchaseOrder.setStatus(transition.NextStatusId);
                        if (!String.IsNullOrEmpty(data.Remarks))
                        {
                            var addRemarks = new Remark()
                            {
                                DocId = getPurchaseOrder.Id,
                                DocType = DocType.PurchaseOrder,
                                Remarks = data.Remarks,
                                UserId = userId
                            };
                            await _unitOfWork.Remarks.Add(addRemarks);
                        }
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            foreach (var line in getPurchaseOrder.PurchaseOrderLines)
                            {
                                line.setStatus(DocumentStatus.Unreconciled);
                            }

                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Purchase Order Approved");
                        }
                        if (transition.NextStatus.State == DocumentStatus.Rejected)
                        {
                            await _unitOfWork.SaveAsync();
                            _unitOfWork.Commit();
                            return new Response<bool>(true, "Purchase Order Rejected");
                        }
                        await _unitOfWork.SaveAsync();
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Purchase Order Reviewed");
                    }
                }

                return new Response<bool>("User does not have allowed role");

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        //Privte Methods for PurchaseOrder

        private async Task<Response<PurchaseOrderDto>> SubmitPO(CreatePurchaseOrderDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PurchaseOrder)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<PurchaseOrderDto>("No workflow found for Purchase Order");
            }

            if (entity.Id == null)
            {
                return await this.SavePO(entity, 6);
            }
            else
            {
                return await this.UpdatePO(entity, 6);
            }
        }

        private async Task<Response<PurchaseOrderDto>> SavePO(CreatePurchaseOrderDto entity, int status)
        {
            if (entity.PurchaseOrderLines.Count() == 0)
                return new Response<PurchaseOrderDto>("Lines are required");

            //Checking duplicate Lines if any
            var duplicates = entity.PurchaseOrderLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<PurchaseOrderDto>("Duplicate Lines found");

            var po = _mapper.Map<PurchaseOrderMaster>(entity);

            //Setting status
            po.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.PurchaseOrder.Add(po);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                po.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<PurchaseOrderDto>(_mapper.Map<PurchaseOrderDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PurchaseOrderDto>(ex.Message);
            }
        }

        private async Task<Response<PurchaseOrderDto>> UpdatePO(CreatePurchaseOrderDto entity, int status)
        {
            if (entity.PurchaseOrderLines.Count() == 0)
                return new Response<PurchaseOrderDto>("Lines are required");

            var specification = new PurchaseOrderSpecs(true);
            var po = await _unitOfWork.PurchaseOrder.GetById((int)entity.Id, specification);

            if (po == null)
                return new Response<PurchaseOrderDto>("Not found");

            if (po.StatusId != 1 && po.StatusId != 2)
                return new Response<PurchaseOrderDto>("Only draft document can be edited");

            //Checking duplicate Lines if any
            var duplicates = entity.PurchaseOrderLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<PurchaseOrderDto>("Duplicate Lines found");

            po.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreatePurchaseOrderDto, PurchaseOrderMaster>(entity, po);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();
              
                //returning response
                return new Response<PurchaseOrderDto>(_mapper.Map<PurchaseOrderDto>(po), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<PurchaseOrderDto>(ex.Message);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        private PurchaseOrderDto MapToValue(PurchaseOrderDto data)
        {
            //Get reconciled grns
            var grnLineReconcileRecord = _unitOfWork.POToGRNLineReconcile
                .Find(new POToGRNLineReconcileSpecs(true, data.Id))
                .GroupBy(x => new { x.GRNId, x.GRN.DocNo })
                .Where(g => g.Count() >= 1)
                .Select(y => new
                {
                    GRNId = y.Key.GRNId,
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
            foreach (var line in data.PurchaseOrderLines)
            {
                // Checking if given amount is greater than unreconciled document amount
                line.ReceivedQuantity = _unitOfWork.POToGRNLineReconcile
                    .Find(new POToGRNLineReconcileSpecs(data.Id, line.Id, line.ItemId, line.WarehouseId))
                    .Sum(p => p.Quantity);

                line.PendingQuantity = line.Quantity - line.ReceivedQuantity;
            }

            return data;
        }
        private List<RemarksDto> ReturningRemarks(PurchaseOrderDto data, DocType docType)
        {
            var remarks = _unitOfWork.Remarks.Find(new RemarksSpecs(data.Id, DocType.PurchaseOrder))
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
        private List<FileUploadDto> ReturningFiles(PurchaseOrderDto data, DocType docType)
        {

            var files = _unitOfWork.Fileupload.Find(new FileUploadSpecs(data.Id, DocType.PurchaseOrder))
                    .Select(e => new FileUploadDto()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DocType = DocType.PurchaseOrder,
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
