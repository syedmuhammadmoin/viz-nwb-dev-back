using Application.Contracts.DTOs;
using Application.Contracts.Filters;
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
    public class CallForQuotationService : ICallForQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CallForQuotationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<CallForQuotationDto>> CreateAsync(CreateCallForQuotationDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitCallForQuotation(entity);
            }
            else
            {
                return await this.SaveCallForQuotation(entity);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<CallForQuotationDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var callForQuotations = await _unitOfWork.CallForQuotation.GetAll(new CallForQuotationSpecs(docDate, states, filter, false));

            if (callForQuotations.Count() == 0)
                return new PaginationResponse<List<CallForQuotationDto>>(_mapper.Map<List<CallForQuotationDto>>(callForQuotations), "List is empty");

            var totalRecords = await _unitOfWork.CallForQuotation.TotalRecord(new CallForQuotationSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<CallForQuotationDto>>(_mapper.Map<List<CallForQuotationDto>>(callForQuotations),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CallForQuotationDto>> GetByIdAsync(int id)
        {
            var specification = new CallForQuotationSpecs(false);

            var callForQuotation = await _unitOfWork.CallForQuotation.GetById(id, specification);

            if (callForQuotation == null)

                return new Response<CallForQuotationDto>("Not found");
            
            var callForQuotationDto = _mapper.Map<CallForQuotationDto>(callForQuotation);

            return new Response<CallForQuotationDto>(callForQuotationDto, "Returning value");
        }

        public async Task<Response<CallForQuotationDto>> UpdateAsync(CreateCallForQuotationDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitCallForQuotation(entity);
            }
            else
            {
                return await this.UpdateCallForQuotation(entity);
            }
        }
        private async Task<Response<CallForQuotationDto>> SubmitCallForQuotation(CreateCallForQuotationDto entity)
        {

            if (entity.Id == null)
            {
                return await this.SaveCallForQuotation(entity);
            }
            else
            {
                return await this.UpdateCallForQuotation(entity);
            }
        }
        private async Task<Response<CallForQuotationDto>> SaveCallForQuotation(CreateCallForQuotationDto entity)
        {
            if (entity.CallForQuotationLines.Count() == 0)
                return new Response<CallForQuotationDto>("Lines are Required");

            var callForQuotation = _mapper.Map<CallForQuotationMaster>(entity);
            if (entity.isSubmit == true)
            {
                callForQuotation.setStatus(DocumentStatus.Submitted);
            }
            else
            {
                callForQuotation.setStatus(DocumentStatus.Draft);
            }
            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.CallForQuotation.Add(callForQuotation);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            callForQuotation.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            return new Response<CallForQuotationDto>(_mapper.Map<CallForQuotationDto>(result), "Created successfully");
        }
        private async Task<Response<CallForQuotationDto>> UpdateCallForQuotation(CreateCallForQuotationDto entity)
        {
            if (entity.CallForQuotationLines.Count() == 0)
                return new Response<CallForQuotationDto>("Lines are required");

            var specification = new CallForQuotationSpecs(true);
            var callForQuotation = await _unitOfWork.CallForQuotation.GetById((int)entity.Id, specification);

            if (callForQuotation == null)
                return new Response<CallForQuotationDto>("Not found");

            if (callForQuotation.State != DocumentStatus.Draft)
                return new Response<CallForQuotationDto>("Only draft document can be edited");

            if (entity.isSubmit == true)
            {
                callForQuotation.setStatus(DocumentStatus.Submitted);
            }
            else
            {
                callForQuotation.setStatus(DocumentStatus.Draft);
            }

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateCallForQuotationDto, CallForQuotationMaster>(entity, callForQuotation);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<CallForQuotationDto>(_mapper.Map<CallForQuotationDto>(callForQuotation), "Updated successfully");
        }
    }
}
