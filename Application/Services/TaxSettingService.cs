using Application.Contracts.DTOs.TaxSetting;
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
    public class TaxSettingService : ITaxSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaxSettingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TaxSettingDto>> CreateAsync(CreateTaxSettingDto entity)
        {
            _unitOfWork.TaxSetting.DeleteAll();      
            var result = await _unitOfWork.TaxSetting.Add(_mapper.Map<TaxSetting>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<TaxSettingDto>(_mapper.Map<TaxSettingDto>(result),"Returning Result");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<List<TaxSettingDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<TaxSettingDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.TaxSetting.GetById(id);
            if (result == null)
                return new Response<TaxSettingDto>("No Setting Found.");
            return new Response<TaxSettingDto>(_mapper.Map<TaxSettingDto>(result),"Returning Setting");
        }

        public Task<Response<TaxSettingDto>> UpdateAsync(CreateTaxSettingDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
