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

        public async Task<Response<RequisitionDto>> CreateAsync(CreateRequisitionDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitRequisition(entity);
            }
            else
            {
                return await this.SaveRequisition(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<RequisitionDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new RequisitionSpecs(filter);
            var requisition = await _unitOfWork.Requisition.GetAll(specification);

            if (requisition.Count() == 0)
                return new PaginationResponse<List<RequisitionDto>>(_mapper.Map<List<RequisitionDto>>(requisition), "List is empty");

            var totalRecords = await _unitOfWork.Requisition.TotalRecord();

            return new PaginationResponse<List<RequisitionDto>>(_mapper.Map<List<RequisitionDto>>(requisition),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<RequisitionDto>> GetByIdAsync(int id)
        {
            var specification = new RequisitionSpecs(false);
            var requisition = await _unitOfWork.Requisition.GetById(id, specification);
            if (requisition == null)
                return new Response<RequisitionDto>("Not found");

            var requisitionDto = _mapper.Map<RequisitionDto>(requisition);

            if ((requisitionDto.State == DocumentStatus.Partial || requisitionDto.State == DocumentStatus.Paid))
            {
                return new Response<RequisitionDto>(MapToValue(requisitionDto), "Returning value");
            }

            requisitionDto.IsAllowedRole = false;
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
                        {
                            requisitionDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<RequisitionDto>(requisitionDto, "Returning value");
        }

        public async Task<Response<RequisitionDto>> UpdateAsync(CreateRequisitionDto entity)
        {
            if (entity.isSubmit)
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
            {
                return new Response<bool>("Requisition with the input id not found");
            }
            if (getRequisition.Status.State == DocumentStatus.Unpaid || getRequisition.Status.State == DocumentStatus.Partial || getRequisition.Status.State == DocumentStatus.Paid)
            {
                return new Response<bool>("Requisition already approved");
            }
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Requisition)).FirstOrDefault();

            if (workflow == null)
            {
                return new Response<bool>("No activated workflow found for this document");
            }
            var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == getRequisition.StatusId && x.Action == data.Action));

            if (transition == null)
            {
                return new Response<bool>("No transition found");
            }
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getRequisition.setStatus(transition.NextStatusId);
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
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        private async Task<Response<RequisitionDto>> SubmitRequisition(CreateRequisitionDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Requisition)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<RequisitionDto>("No workflow found for Requisition");
            }

            if (entity.Id == null)
            {
                return await this.SaveRequisition(entity, 6);
            }
            else
            {
                return await this.UpdateRequisition(entity, 6);
            }
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

            var requisition = _mapper.Map<RequisitionMaster>(entity);

            //Setting status
            requisition.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
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
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<RequisitionDto>(ex.Message);
            }
        }

        private async Task<Response<RequisitionDto>> UpdateRequisition(CreateRequisitionDto entity, int status)
        {
            if (entity.RequisitionLines.Count() == 0)
                return new Response<RequisitionDto>("Lines are required");

            var specification = new RequisitionSpecs(true);
            var requisition = await _unitOfWork.Requisition.GetById((int)entity.Id, specification);

            if (requisition == null)
                return new Response<RequisitionDto>("Not found");

            if (requisition.StatusId != 1 && requisition.StatusId != 2)
                return new Response<RequisitionDto>("Only draft document can be edited");
            
            //Checking duplicate Lines if any
            var duplicates = entity.RequisitionLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<RequisitionDto>("Duplicate Lines found");
            //Setting status
            requisition.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateRequisitionDto, RequisitionMaster>(entity, requisition);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<RequisitionDto>(_mapper.Map<RequisitionDto>(requisition), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<RequisitionDto>(ex.Message);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
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
            var getReference = new List<IssuanceAndRequisitionReferencesDto>();
            if (grnLineReconcileRecord.Any())
            {
                foreach (var line in grnLineReconcileRecord)
                {
                    getReference.Add(new IssuanceAndRequisitionReferencesDto
                    {
                        DocId = line.IssuanceId,
                        DocNo = line.DocNo
                    });
                }
            }
            data.References = getReference;

            // Get pending & received quantity...
            foreach (var line in data.RequisitionLines)
            {
                // Checking if given amount is greater than unreconciled document amount
                line.IssuedQuantity = _unitOfWork.RequisitionToIssuanceLineReconcile
                    .Find(new RequisitionToIssuanceLineReconcileSpecs(data.Id, line.Id, line.ItemId, line.WarehouseId))
                    .Sum(p => p.Quantity);

                line.PendingQuantity = line.Quantity - line.IssuedQuantity;
            }

            return data;
        }
    }
}
