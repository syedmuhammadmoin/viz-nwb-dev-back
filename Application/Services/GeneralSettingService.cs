using Application.Contracts.DTOs.GeneralSetting;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GeneralSettingService : IGeneralSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GeneralSettingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<GeneralSettingDto>> CreateAsync(CreateGeneralSettingDto entity)
        {
            _unitOfWork.GeneralSetting.DeleteAll();
            var result = await _unitOfWork.GeneralSetting.Add(_mapper.Map<GeneralSettingEntity>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<GeneralSettingDto>(_mapper.Map<GeneralSettingDto>(result), "Created Successfully.");

        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<List<GeneralSettingDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<GeneralSettingDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.GeneralSetting.GetById(id);
            if (result == null)
                return new Response<GeneralSettingDto>("Setting Not Found");
            return new Response<GeneralSettingDto>(_mapper.Map<GeneralSettingDto>(result), "Returning Setting.");
        }

        public Task<Response<GeneralSettingDto>> UpdateAsync(CreateGeneralSettingDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
