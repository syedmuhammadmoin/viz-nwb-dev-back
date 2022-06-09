using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
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

    public class WorkFlowStatusService : IWorkFlowStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WorkFlowStatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<WorkFlowStatusDto>> CreateAsync(CreateWorkFlowStatusDto entity)
        {
            var status = _mapper.Map<WorkFlowStatus>(entity);
            var result = await _unitOfWork.WorkFlowStatus.Add(status);
            await _unitOfWork.SaveAsync();

            return new Response<WorkFlowStatusDto>(_mapper.Map<WorkFlowStatusDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<WorkFlowStatusDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new WorkFlowStatusSpecs(filter);
            var status = await _unitOfWork.WorkFlowStatus.GetAll(specification);

            if (!status.Any())
                return new PaginationResponse<List<WorkFlowStatusDto>>(_mapper.Map<List<WorkFlowStatusDto>>(status), "List is empty");

            var totalRecords = await _unitOfWork.WorkFlowStatus.TotalRecord(new WorkFlowStatusSpecs());

            return new PaginationResponse<List<WorkFlowStatusDto>>(_mapper.Map<List<WorkFlowStatusDto>>(status), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<WorkFlowStatusDto>> GetByIdAsync(int id)
        {
            var status = await _unitOfWork.WorkFlowStatus.GetById(id);
            if (status == null)
                return new Response<WorkFlowStatusDto>("Not found");

            return new Response<WorkFlowStatusDto>(_mapper.Map<WorkFlowStatusDto>(status), "Returning value");
        }

        public async Task<Response<WorkFlowStatusDto>> UpdateAsync(CreateWorkFlowStatusDto entity)
        {
            var status = await _unitOfWork.WorkFlowStatus.GetById((int)entity.Id);

            if (status == null)
                return new Response<WorkFlowStatusDto>("Not found");

            if (status.Type != Domain.Constants.StatusType.Custom)
                return new Response<WorkFlowStatusDto>("Only user defined can be edited");


            //For updating data
            _mapper.Map<CreateWorkFlowStatusDto, WorkFlowStatus>(entity, status);
            await _unitOfWork.SaveAsync();
            return new Response<WorkFlowStatusDto>(_mapper.Map<WorkFlowStatusDto>(status), "Updated successfully");
        }

        public async Task<Response<List<WorkFlowStatusDto>>> GetStatusDropDown()
        {
            var status = await _unitOfWork.WorkFlowStatus.GetAll(new WorkFlowStatusSpecs());
            if (!status.Any())
                return new Response<List<WorkFlowStatusDto>>("List is empty");

            return new Response<List<WorkFlowStatusDto>>(_mapper.Map<List<WorkFlowStatusDto>>(status), "Returning List");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
