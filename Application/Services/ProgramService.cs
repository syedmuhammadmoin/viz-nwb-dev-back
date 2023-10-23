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
    public class ProgramService : IProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<ProgramDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Program.GetAll(new ProgramSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<ProgramDto>>(_mapper.Map<List<ProgramDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Program.TotalRecord(new ProgramSpecs(filter, true));
            return new PaginationResponse<List<ProgramDto>>(_mapper.Map<List<ProgramDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ProgramDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Program.GetById(id, new ProgramSpecs(false));
            if (result == null)
                return new Response<ProgramDto>("Not found");

            return new Response<ProgramDto>(_mapper.Map<ProgramDto>(result), "Returning value");
        }

        public async Task<Response<List<ProgramDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Program.GetAll();
            if (!result.Any())
                return new Response<List<ProgramDto>>(null, "List is empty");

            return new Response<List<ProgramDto>>(_mapper.Map<List<ProgramDto>>(result), "Returning List");
        }

        public async Task<Response<ProgramDto>> CreateAsync(CreateProgramDto entity)
        {
            var result = await _unitOfWork.Program.Add(_mapper.Map<Program>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<ProgramDto>(_mapper.Map<ProgramDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<ProgramDto>> UpdateAsync(CreateProgramDto entity)
        {
            var result = await _unitOfWork.Program.GetById((int)entity.Id, new ProgramSpecs(true));
            if (result == null)
                return new Response<ProgramDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<ProgramDto>(_mapper.Map<ProgramDto>(result), "Updated successfully");
        }

    }
}
