using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Infrastructure.Specifications;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DesignationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DesignationDto>> CreateAsync(DesignationDto entity)
        {
            var designation = _mapper.Map<Designation>(entity);
            var getDesignation = await _unitOfWork.Designation.GetById((int)entity.Id);

            if (getDesignation != null)
            {
                _mapper.Map<DesignationDto, Designation>(entity, getDesignation);
                await _unitOfWork.SaveAsync();

                return new Response<DesignationDto>(_mapper.Map<DesignationDto>(getDesignation), "Updated successfully");
            }
            else
            {
                await _unitOfWork.Designation.Add(designation);
                await _unitOfWork.SaveAsync();
            }

            return new Response<DesignationDto>(_mapper.Map<DesignationDto>(getDesignation), "Created successfully");
        }

        public async Task<PaginationResponse<List<DesignationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new DesignationSpecs(filter);
            var designations = await _unitOfWork.Designation.GetAll(specification);

            if (designations.Count() == 0)
                return new PaginationResponse<List<DesignationDto>>(_mapper.Map<List<DesignationDto>>(designations), "List is empty");

            var totalRecords = await _unitOfWork.Designation.TotalRecord();

            return new PaginationResponse<List<DesignationDto>>(_mapper.Map<List<DesignationDto>>(designations), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DesignationDto>> GetByIdAsync(int id)
        {
            var designation = await _unitOfWork.Designation.GetById(id);
            if (designation == null)
                return new Response<DesignationDto>("Not found");

            return new Response<DesignationDto>(_mapper.Map<DesignationDto>(designation), "Returning value");
        }

        public async Task<Response<List<DesignationDto>>> GetDesignationDropDown()
        {
            var designations = await _unitOfWork.Designation.GetAll();
            if (!designations.Any())
                return new Response<List<DesignationDto>>("List is empty");

            return new Response<List<DesignationDto>>(_mapper.Map<List<DesignationDto>>(designations), "Returning List");
        }

        public Task<Response<DesignationDto>> UpdateAsync(DesignationDto entity)
        {
            throw new NotImplementedException();
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
