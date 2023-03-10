using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class DepreciationModelService : IDepreciationModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepreciationModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<DepreciationModelDto>> CreateAsync(CreateDepreciationModelDto entity)
        {
            if (entity.ModelType == DepreciationMethod.Declining && entity.DecliningRate == 0)
            {
                return new Response<DepreciationModelDto>("Declining rate must be greater than zero");
            }
            var depreciationModel = _mapper.Map<DepreciationModel>(entity);
            //Saving in table
            await _unitOfWork.DepreciationModel.Add(depreciationModel);
            await _unitOfWork.SaveAsync();
            
            //returning response
            return new Response<DepreciationModelDto>(_mapper.Map<DepreciationModelDto>(depreciationModel), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<DepreciationModelDto>> UpdateAsync(CreateDepreciationModelDto entity)
        {
            if (entity.ModelType == DepreciationMethod.Declining && entity.DecliningRate == 0)
            {
                return new Response<DepreciationModelDto>("Declining rate must be greater than zero");
            }
            
            var result = await _unitOfWork.DepreciationModel.GetById((int)entity.Id);
            if (result == null)
                return new Response<DepreciationModelDto>("Not found");
            
            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();

            //returning response
            return new Response<DepreciationModelDto>(_mapper.Map<DepreciationModelDto>(result), "Updated successfully");
        }

        public async Task<PaginationResponse<List<DepreciationModelDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var depreciation = await _unitOfWork.DepreciationModel
                .GetAll(new DepreciationModelSpecs(filter, false));
            
            if (depreciation.Count() == 0)
                return new PaginationResponse<List<DepreciationModelDto>>(_mapper.Map<List<DepreciationModelDto>>(depreciation), "List is empty");

            var totalRecords = await _unitOfWork.DepreciationModel.TotalRecord(new DepreciationModelSpecs(filter, true));
            return new PaginationResponse<List<DepreciationModelDto>>(_mapper.Map<List<DepreciationModelDto>>(depreciation),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<DepreciationModelDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.DepreciationModel
                .GetById(id, new DepreciationModelSpecs());
            if (result == null)
                return new Response<DepreciationModelDto>("Not found");

            return new Response<DepreciationModelDto>(_mapper.Map<DepreciationModelDto>(result), "Returning value"); 
        }

        public async Task<Response<List<DepreciationModelDto>>> GetDropDown()
        {
            var result = await _unitOfWork.DepreciationModel.GetAll();
            if (!result.Any())
                return new Response<List<DepreciationModelDto>>("List is empty");

            return new Response<List<DepreciationModelDto>>(_mapper.Map<List<DepreciationModelDto>>(result), "Returning List");
        }
    
    }
}
