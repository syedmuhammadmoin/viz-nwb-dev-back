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
    public class CampusService : ICampusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CampusDto>> CreateAsync(CampusDto entity)
        {
            var campus = _mapper.Map<Campus>(entity);
            var result = await _unitOfWork.Campus.Add(campus);
            await _unitOfWork.SaveAsync();

            return new Response<CampusDto>(_mapper.Map<CampusDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<CampusDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new CampusSpecs(filter);
            var campus = await _unitOfWork.Campus.GetAll(specification);

            if (campus.Count() == 0)
                return new PaginationResponse<List<CampusDto>>("List is empty");

            var totalRecords = await _unitOfWork.Campus.TotalRecord();

            return new PaginationResponse<List<CampusDto>>(_mapper.Map<List<CampusDto>>(campus), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CampusDto>> GetByIdAsync(int id)
        {
            var campus = await _unitOfWork.Campus.GetById(id);
            if (campus == null)
                return new Response<CampusDto>("Not found");

            return new Response<CampusDto>(_mapper.Map<CampusDto>(campus), "Returning value");
        }

        public async Task<Response<CampusDto>> UpdateAsync(CampusDto entity)
        {
            var campus = await _unitOfWork.Campus.GetById((int)entity.Id);

            if (campus == null)
                return new Response<CampusDto>("Not found");

            //For updating data
            _mapper.Map<CampusDto, Campus>(entity, campus);
            await _unitOfWork.SaveAsync();
            return new Response<CampusDto>(_mapper.Map<CampusDto>(campus), "Updated successfully");
        }

        public async Task<Response<List<CampusDto>>> GetCampusDropDown()
        {
            var campuses = await _unitOfWork.Campus.GetAll();
            if (!campuses.Any())
                return new Response<List<CampusDto>>("List is empty");

            return new Response<List<CampusDto>>(_mapper.Map<List<CampusDto>>(campuses), "Returning List");
        }
    }
}
