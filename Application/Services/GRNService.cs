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
            var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
            _unitOfWork.CreateTransaction();
            try
            {
                foreach (var role in currentUserRoles)
                {
                    if (transition.AllowedRole.Name == role)
                    {
                        getGRN.setStatus(transition.NextStatusId);
                        if (transition.NextStatus.State == DocumentStatus.Unpaid)
                        {
                            await _unitOfWork.SaveAsync();
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
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public async Task<Response<GRNDto>> CreateAsync(CreateGRNDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitGRN(entity);
            }
            else
            {
                return await this.SaveGRN(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<GRNDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new GRNSpecs(filter);
            var gRN = await _unitOfWork.GRN.GetAll(specification);

            if (gRN.Count() == 0)
                return new PaginationResponse<List<GRNDto>>(_mapper.Map<List<GRNDto>>(gRN), "List is empty");

            var totalRecords = await _unitOfWork.GRN.TotalRecord();

            return new PaginationResponse<List<GRNDto>>(_mapper.Map<List<GRNDto>>(gRN),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<GRNDto>> GetByIdAsync(int id)
        {
            var specification = new GRNSpecs(false);
            var gRN = await _unitOfWork.GRN.GetById(id, specification);
            if (gRN == null)
                return new Response<GRNDto>("Not found");

            var requisitionDto = _mapper.Map<GRNDto>(gRN);

            requisitionDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GRN)).FirstOrDefault();


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
            return new Response<GRNDto>(requisitionDto, "Returning value");

        }

        public async Task<Response<GRNDto>> UpdateAsync(CreateGRNDto entity)
        {
            if (entity.isSubmit)
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
            //setting PurchaseOrderId
            var getPO = await _unitOfWork.PurchaseOrder.GetById(entity.PurchaseOrderId);

            if (getPO == null)
                return new Response<GRNDto>("Purchase Order is required");

            if (entity.GRNLines.Count() == 0)
                return new Response<GRNDto>("Lines are required");

            var gRN = _mapper.Map<GRNMaster>(entity);

            //Setting PurchaseId
            gRN.setPurchaseOrderId(getPO.Id);

            //Setting status
            gRN.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.GRN.Add(gRN);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                gRN.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<GRNDto>(_mapper.Map<GRNDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<GRNDto>(ex.Message);
            }
        }

        private async Task<Response<GRNDto>> UpdateGRN(CreateGRNDto entity, int status)
        {
            //setting PurchaseOrderId
            var getPO = await _unitOfWork.PurchaseOrder.GetById(entity.PurchaseOrderId);

            if (getPO == null)
                return new Response<GRNDto>("Purchase Order not found");

            if (entity.GRNLines.Count() == 0)
                return new Response<GRNDto>("Lines are required");

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
            try
            {
                //For updating data
                _mapper.Map<CreateGRNDto, GRNMaster>(entity, gRN);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<GRNDto>(_mapper.Map<GRNDto>(gRN), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<GRNDto>(ex.Message);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
