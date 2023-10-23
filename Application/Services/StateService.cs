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
    public class StateService : IStateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<StateDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.State.GetAll(new StateSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<StateDto>>(_mapper.Map<List<StateDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.State.TotalRecord(new StateSpecs(filter, true));
            return new PaginationResponse<List<StateDto>>(_mapper.Map<List<StateDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<StateDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.State.GetById(id, new StateSpecs());
            if (result == null)
                return new Response<StateDto>("Not found");

            return new Response<StateDto>(_mapper.Map<StateDto>(result), "Returning value");
        }

        public async Task<Response<List<StateDto>>> GetDropDown()
        {
            var result = await _unitOfWork.State.GetAll();
            if (!result.Any())
                return new Response<List<StateDto>>(null, "List is empty");

            return new Response<List<StateDto>>(_mapper.Map<List<StateDto>>(result), "Returning List");
        }


        public async Task<Response<List<StateDto>>> GetByCountry(int countryId)
        {
            var result = await _unitOfWork.State.GetAll(new StateSpecs(countryId));
            if (!result.Any())
                return new Response<List<StateDto>>(null, "List is empty");

            return new Response<List<StateDto>>(_mapper.Map<List<StateDto>>(result), "Returning List");
        }

        public async Task<Response<StateDto>> CreateAsync(CreateStateDto entity)
        {
            var result = await _unitOfWork.State.Add(_mapper.Map<State>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<StateDto>(_mapper.Map<StateDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<StateDto>> UpdateAsync(CreateStateDto entity)
        {
            var result = await _unitOfWork.State.GetById((int)entity.Id);
            if (result == null)
                return new Response<StateDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<StateDto>(_mapper.Map<StateDto>(result), "Updated successfully");
        }

    }
}
