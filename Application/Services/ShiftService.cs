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
    public class ShiftService : IShiftService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShiftService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<ShiftDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Shift.GetAll(new ShiftSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<ShiftDto>>(_mapper.Map<List<ShiftDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Shift.TotalRecord(new ShiftSpecs(filter, true));
            return new PaginationResponse<List<ShiftDto>>(_mapper.Map<List<ShiftDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ShiftDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Shift.GetById(id);
            if (result == null)
                return new Response<ShiftDto>("Not found");

            return new Response<ShiftDto>(_mapper.Map<ShiftDto>(result), "Returning value");
        }

        public async Task<Response<List<ShiftDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Shift.GetAll();
            if (!result.Any())
                return new Response<List<ShiftDto>>(null, "List is empty");

            return new Response<List<ShiftDto>>(_mapper.Map<List<ShiftDto>>(result), "Returning List");
        }

        public async Task<Response<ShiftDto>> CreateAsync(ShiftDto entity)
        {
            var result = await _unitOfWork.Shift.Add(_mapper.Map<Shift>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<ShiftDto>(_mapper.Map<ShiftDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<ShiftDto>> UpdateAsync(ShiftDto entity)
        {
            var result = await _unitOfWork.Shift.GetById((int)entity.Id);
            if (result == null)
                return new Response<ShiftDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<ShiftDto>(_mapper.Map<ShiftDto>(result), "Updated successfully");
        }
    }
}
