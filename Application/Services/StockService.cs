﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<StockDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new StockSpecs(filter);
            var stock = await _unitOfWork.Stock.GetAll(specification);

            if (stock.Count() == 0)
                return new PaginationResponse<List<StockDto>>(_mapper.Map<List<StockDto>>(stock), "List is empty");

            var totalRecords = await _unitOfWork.Stock.TotalRecord(specification);

            return new PaginationResponse<List<StockDto>>(_mapper.Map<List<StockDto>>(stock),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }
    }
}