using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class DegreeService : IDegreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DegreeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<DegreeDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Degree.GetAll(new DegreeSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<DegreeDto>>(_mapper.Map<List<DegreeDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Degree.TotalRecord(new DegreeSpecs(filter, true));
            return new PaginationResponse<List<DegreeDto>>(_mapper.Map<List<DegreeDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DegreeDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Degree.GetById(id);
            if (result == null)
                return new Response<DegreeDto>("Not found");

            return new Response<DegreeDto>(_mapper.Map<DegreeDto>(result), "Returning value");
        }

        public async Task<Response<List<DegreeDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Degree.GetAll();
            if (!result.Any())
                return new Response<List<DegreeDto>>(null, "List is empty");

            return new Response<List<DegreeDto>>(_mapper.Map<List<DegreeDto>>(result), "Returning List");
        }

        public async Task<Response<DegreeDto>> CreateAsync(DegreeDto entity)
        {
            var result = await _unitOfWork.Degree.Add(_mapper.Map<Degree>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<DegreeDto>(_mapper.Map<DegreeDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DegreeDto>> UpdateAsync(DegreeDto entity)
        {
            var result = await _unitOfWork.Degree.GetById((int)entity.Id);
            if (result == null)
                return new Response<DegreeDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<DegreeDto>(_mapper.Map<DegreeDto>(result), "Updated successfully");
        }

    }
}
