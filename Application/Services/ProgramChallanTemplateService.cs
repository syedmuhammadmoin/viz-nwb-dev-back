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
    public class ProgramChallanTemplateService : IProgramChallanTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramChallanTemplateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<ProgramChallanTemplateDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.ProgramChallanTemplate.GetAll(new ProgramChallanTemplateSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<ProgramChallanTemplateDto>>(_mapper.Map<List<ProgramChallanTemplateDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.ProgramChallanTemplate.TotalRecord(new ProgramChallanTemplateSpecs(filter, true));
            return new PaginationResponse<List<ProgramChallanTemplateDto>>(_mapper.Map<List<ProgramChallanTemplateDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ProgramChallanTemplateDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.ProgramChallanTemplate.GetById(id, new ProgramChallanTemplateSpecs(false));
            if (result == null)
                return new Response<ProgramChallanTemplateDto>("Not found");

            return new Response<ProgramChallanTemplateDto>(_mapper.Map<ProgramChallanTemplateDto>(result), "Returning value");
        }

        public async Task<Response<ProgramChallanTemplateDto>> CreateAsync(CreateProgramChallanTemplateDto entity)
        {
            var result = await _unitOfWork.ProgramChallanTemplate.Add(_mapper.Map<ProgramChallanTemplateMaster>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<ProgramChallanTemplateDto>(_mapper.Map<ProgramChallanTemplateDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<ProgramChallanTemplateDto>> UpdateAsync(CreateProgramChallanTemplateDto entity)
        {
            var result = await _unitOfWork.ProgramChallanTemplate.GetById((int)entity.Id, new ProgramChallanTemplateSpecs(true));
            if (result == null)
                return new Response<ProgramChallanTemplateDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<ProgramChallanTemplateDto>(_mapper.Map<ProgramChallanTemplateDto>(result), "Updated successfully");
        }
    }
}
