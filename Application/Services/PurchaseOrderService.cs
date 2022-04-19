﻿using Application.Contracts.DTOs;
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
            var jvs = await _unitOfWork.PurchaseOrder.GetAll(specification);

            if (jvs.Count() == 0)
                return new PaginationResponse<List<PurchaseOrderDto>>("List is empty");

            var totalRecords = await _unitOfWork.PurchaseOrder.TotalRecord();

            return new PaginationResponse<List<PurchaseOrderDto>>(_mapper.Map<List<PurchaseOrderDto>>(jvs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<PurchaseOrderDto>> GetByIdAsync(int id)
        {
            var specification = new PurchaseOrderSpecs(false);
            var dbn = await _unitOfWork.PurchaseOrder.GetById(id, specification);
            if (dbn == null)
                return new Response<PurchaseOrderDto>("Not found");

            var debitNoteDto = _mapper.Map<PurchaseOrderDto>(dbn);

            debitNoteDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.PurchaseOrder)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == debitNoteDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            debitNoteDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<PurchaseOrderDto>(debitNoteDto, "Returning value");
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

        public Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            throw new NotImplementedException();
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

            var dbn = _mapper.Map<PurchaseOrderMaster>(entity);

            //setting BusinessPartnerPayable
            var er = await _unitOfWork.BusinessPartner.GetById(entity.VendorId);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.PurchaseOrder.Add(dbn);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                dbn.CreateDocNo();
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
            var dbn = await _unitOfWork.PurchaseOrder.GetById((int)entity.Id, specification);

            if (dbn == null)
                return new Response<PurchaseOrderDto>("Not found");

            if (dbn.StatusId != 1 && dbn.StatusId != 2)
                return new Response<PurchaseOrderDto>("Only draft document can be edited");

            //setting BusinessPartnerPayable
            var er = await _unitOfWork.BusinessPartner.GetById(entity.VendorId);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreatePurchaseOrderDto, PurchaseOrderMaster>(entity, dbn);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<PurchaseOrderDto>(_mapper.Map<PurchaseOrderDto>(dbn), "Created successfully");
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
