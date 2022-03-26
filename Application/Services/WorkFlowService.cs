using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkFlowService : IWorkFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WorkFlowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<WorkFlowDto>> CreateAsync(CreateWorkFlowDto entity)
        {
            if (entity.IsActive)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(entity.DocType)).FirstOrDefault();
                if (checkingActiveWorkFlows != null)
                {
                    return new Response<WorkFlowDto>("Workflow already activated for this document");
                }
            }
            var workFlow = _mapper.Map<WorkFlowMaster>(entity);

            var result = await _unitOfWork.WorkFlow.Add(workFlow);
            await _unitOfWork.SaveAsync();

            return new Response<WorkFlowDto>(_mapper.Map<WorkFlowDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<WorkFlowDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new WorkFlowSpecs(filter);
            var WorkFlow = await _unitOfWork.WorkFlow.GetAll(specification);

            if (!WorkFlow.Any())
                return new PaginationResponse<List<WorkFlowDto>>("List is empty");

            var totalRecords = await _unitOfWork.WorkFlow.TotalRecord();

            return new PaginationResponse<List<WorkFlowDto>>(_mapper.Map<List<WorkFlowDto>>(WorkFlow), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<WorkFlowDto>> GetByIdAsync(int id)
        {
            var specification = new WorkFlowSpecs(false);
            var WorkFlow = await _unitOfWork.WorkFlow.GetById(id, specification);
            if (WorkFlow == null)
                return new Response<WorkFlowDto>("Not found");

            return new Response<WorkFlowDto>(_mapper.Map<WorkFlowDto>(WorkFlow), "Returning value");
        }

        public async Task<Response<WorkFlowDto>> UpdateAsync(CreateWorkFlowDto entity)
        {
            var WorkFlow = await _unitOfWork.WorkFlow.GetById((int)entity.Id, new WorkFlowSpecs(true));

            if (WorkFlow == null)
                return new Response<WorkFlowDto>("Not found");

            if (entity.IsActive)
            {
                var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(entity.DocType, (int)entity.Id)).FirstOrDefault();
                if (checkingActiveWorkFlows != null)
                {
                    return new Response<WorkFlowDto>("Workflow already activated for this document");
                }
            }

            if (entity.DocType == DocType.Invoice)
            {
                var checkingInvoice = _unitOfWork.Invoice.Find(new InvoiceSpecs()).ToList();
                
                if (checkingInvoice.Count != 0)
                {
                    return new Response<WorkFlowDto>("Invoice is pending for this workflow");
                }
            }

            return new Response<WorkFlowDto>(_mapper.Map<WorkFlowDto>(WorkFlow), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
