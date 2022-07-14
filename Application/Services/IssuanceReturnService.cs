using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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

        public Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<IssuanceReturnDto>> CreateAsync(CreateIssuanceReturnDto entity)
        {
            if (entity.isSubmit)
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

            var specification = new IssuanceReturnSpecs(docDate, states, filter);
            var issuanceReturn = await _unitOfWork.IssuanceReturn.GetAll(specification);

            if (issuanceReturn.Count() == 0)
                return new PaginationResponse<List<IssuanceReturnDto>>(_mapper.Map<List<IssuanceReturnDto>>(issuanceReturn), "List is empty");

            var totalRecords = await _unitOfWork.IssuanceReturn.TotalRecord(specification);

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

        public Task<Response<IssuanceReturnDto>> UpdateAsync(CreateIssuanceReturnDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
