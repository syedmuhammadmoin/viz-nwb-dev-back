using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencyService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        async Task<Response<CurrencyDto>> ICrudService<CreateCurrencyDto, CurrencyDto, int, CurrencyFilter>.CreateAsync(CreateCurrencyDto entity)
        {
            
            var currency = _mapper.Map<Currency>(entity);
            currency.SetCurrencyRate();
            await _unitOfWork.GetRepository<Currency>().Add(currency);
            await _unitOfWork.SaveAsync();
            return new Response<CurrencyDto>(_mapper.Map<CurrencyDto>(currency), ResponseMessages.CreatedSuccessfully);
        }

        Task<Response<int>> ICrudService<CreateCurrencyDto, CurrencyDto, int, CurrencyFilter>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        async Task<PaginationResponse<List<CurrencyDto>>> ICrudService<CreateCurrencyDto, CurrencyDto, int, CurrencyFilter>.GetAllAsync(CurrencyFilter filter)
        {
            var specification = new CurrencySpec(filter, false);
            var currency = await _unitOfWork.GetRepository<Currency>().GetAll(specification);
            if (currency.Count() == 0)
                new PaginationResponse<List<CurrencyDto>>(_mapper.Map<List<CurrencyDto>>(currency), ResponseMessages.EmptyList);

            var paginationSpecification = new CurrencySpec(filter, true);
            var totalRecord = await _unitOfWork.GetRepository<Currency>().TotalRecord(paginationSpecification);
            var data = _mapper.Map<List<CurrencyDto>>(currency);
            return new PaginationResponse<List<CurrencyDto>>(data, filter.PageStart, filter.PageEnd, totalRecord, ResponseMessages.ReturnList);
        }

        async Task<Response<CurrencyDto>> ICrudService<CreateCurrencyDto, CurrencyDto, int, CurrencyFilter>.GetByIdAsync(int id)
        {
            var specification = new CurrencySpec();
            var currency = await _unitOfWork.GetRepository<Currency>().GetById(id, specification);
            if (currency == null)
            {
                return new Response<CurrencyDto>(ResponseMessages.NotFound);
            }

            return new Response<CurrencyDto>(_mapper.Map<CurrencyDto>(currency), ResponseMessages.RetrieveSuccessfully);
        }

        async Task<Response<CurrencyDto>> ICrudService<CreateCurrencyDto, CurrencyDto, int, CurrencyFilter>.UpdateAsync(CreateCurrencyDto entity)
        {
            var currency = await _unitOfWork.GetRepository<Currency>().GetById((int)entity.Id);
            if (currency == null)
                return new Response<CurrencyDto>(ResponseMessages.NotFound);
            _mapper.Map<CreateCurrencyDto, Currency>(entity, currency);
            currency.SetCurrencyRate();
            await _unitOfWork.SaveAsync();
            return new Response<CurrencyDto>(_mapper.Map<CurrencyDto>(currency), ResponseMessages.UpdatedSuccessfully);
        }



        async Task<Response<List<CurrencyDto>>> ICurrencyService.GetDropDown()
        {
            var result = await _unitOfWork.GetRepository<Currency>().GetAll();
            if (!result.Any())
                return new Response<List<CurrencyDto>>(null, ResponseMessages.EmptyList);

            var data = _mapper.Map<List<CurrencyDto>>(result);
            return new Response<List<CurrencyDto>>(data, ResponseMessages.ReturnList);
        }
    }
}
