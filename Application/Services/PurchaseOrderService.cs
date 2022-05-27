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

        public async Task<PaginationResponse<List<PurchaseOrderDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new PurchaseOrderSpecs(filter);
            var po = await _unitOfWork.PurchaseOrder.GetAll(specification);

            if (po.Count() == 0)
                return new PaginationResponse<List<PurchaseOrderDto>>(_mapper.Map<List<PurchaseOrderDto>>(po), "List is empty");

            var totalRecords = await _unitOfWork.PurchaseOrder.TotalRecord();

            return new PaginationResponse<List<PurchaseOrderDto>>(_mapper.Map<List<PurchaseOrderDto>>(po),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<PurchaseOrderDto>> GetByIdAsync(int id)
        {
            var specification = new PurchaseOrderSpecs(false);
            var po = await _unitOfWork.PurchaseOrder.GetById(id, specification);
            if (po == null)
                return new Response<PurchaseOrderDto>("Not found");

            var requisitionDto = _mapper.Map<PurchaseOrderDto>(po);

            requisitionDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PurchaseOrder)).FirstOrDefault();


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
            return new Response<PurchaseOrderDto>(requisitionDto, "Returning value");
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
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getPurchaseOrder.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
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

    }
}
