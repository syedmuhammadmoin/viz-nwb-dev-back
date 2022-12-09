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
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            throw new NotImplementedException();
        }
        public async Task<Response<RequestDto>> CreateAsync(CreateRequestDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitRequest(entity);
            }
            else
            {
                return await this.SaveRequest(entity, 1);
            }
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<PaginationResponse<List<RequestDto>>> GetAllAsync(TransactionFormFilter filter)
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

            var request = await _unitOfWork.Request.GetAll(new RequestSpecs(docDate, states, filter, false));

            if (request.Count() == 0)
            {
                return new PaginationResponse<List<RequestDto>>(_mapper.Map<List<RequestDto>>(request), "List is empty");
            }
            var totalRecords = await _unitOfWork.Request.TotalRecord(new RequestSpecs(docDate, states, filter, true));

            return new PaginationResponse<List<RequestDto>>(_mapper.Map<List<RequestDto>>(request),
                    filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }
        public async Task<Response<RequestDto>> GetByIdAsync(int id)
        {
            var specification = new RequestSpecs(false);
            var request = await _unitOfWork.Request.GetById(id, specification);
            if (request == null)
                return new Response<RequestDto>("Not found");

            var requestDto = _mapper.Map<RequestDto>(request);
            requestDto.IsAllowedRole = false;

            return new Response<RequestDto>(requestDto, "Returning value");
        }
        private async Task<Response<RequestDto>> SubmitRequest(CreateRequestDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.Request)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<RequestDto>("No workflow found for Request");
            }

            if (entity.Id == null)
            {
                return await this.SaveRequest(entity, 7);
            }
            else
            {
                return await this.UpdateRequest(entity, 7);
            }
        }
        private async Task<Response<RequestDto>> SaveRequest(CreateRequestDto entity, int status)
        {
            if (entity.RequestLines.Count() == 0)
                return new Response<RequestDto>("Lines are Required");

            var duplicates = entity.RequestLines.GroupBy(x => new { x.ItemDescription })
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();


            if (duplicates.Any())
                return new Response<RequestDto>("Duplicate Lines found");

            var request = _mapper.Map<RequestMaster>(entity);

            //Setting status
            request.setStatus(status);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.Request.Add(request);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            request.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            return new Response<RequestDto>(_mapper.Map<RequestDto>(result), "Created successfully");
        }
        public async Task<Response<RequestDto>> UpdateAsync(CreateRequestDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitRequest(entity);
            }
            else
            {
                return await this.UpdateRequest(entity, 1);
            }
        }
        private async Task<Response<RequestDto>> UpdateRequest(CreateRequestDto entity, int status)
        {
            if (entity.RequestLines.Count() == 0)
                return new Response<RequestDto>("Lines are required");

            var specification = new RequestSpecs(true);
            var request = await _unitOfWork.Request.GetById((int)entity.Id, specification);

            if (request == null)
                return new Response<RequestDto>("Not found");

            if (request.StatusId != 1 && request.StatusId != 2)
                return new Response<RequestDto>("Only draft document can be edited");

            var duplicates = entity.RequestLines.GroupBy(x => new { x.ItemDescription })
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

            if (duplicates.Any())
                return new Response<RequestDto>("Duplicate Lines found");


            request.setStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateRequestDto, RequestMaster>(entity, request);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<RequestDto>(_mapper.Map<RequestDto>(request), "Updated successfully");

        }

    }
}
