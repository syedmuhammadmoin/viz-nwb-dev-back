using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QuotationComparativeService : IQuotationComparativeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuotationComparativeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public async Task<Response<QuotationComparativeDto>> CreateAsync(CreateQuotationComparativeDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitQuotationComparative(entity);
            }
            else
            {
                return await this.SaveQuotationComparative(entity);
            }
        }
        
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<PaginationResponse<List<QuotationComparativeDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var DocDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                DocDate.Add(filter.DocDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }

            var quotationComparative = await _unitOfWork.QuotationComparative.GetAll(new QuotationComparativeSpecs(DocDate, states, filter, false));

            if (quotationComparative.Count() == 0)
                return new PaginationResponse<List<QuotationComparativeDto>>(_mapper.Map<List<QuotationComparativeDto>>(quotationComparative), "List is empty");

            var totalRecords = await _unitOfWork.QuotationComparative.TotalRecord(new QuotationComparativeSpecs(DocDate, states, filter, true));

            return new PaginationResponse<List<QuotationComparativeDto>>(_mapper.Map<List<QuotationComparativeDto>>(quotationComparative),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }
        
        public async Task<Response<QuotationComparativeDto>> GetByIdAsync(int id)
        {
            var specification = new QuotationComparativeSpecs(false);
            var quotationComparative = await _unitOfWork.QuotationComparative.GetById(id, specification);
            if (quotationComparative == null)
                return new Response<QuotationComparativeDto>("Not found");

            var quotationComparativeDto = _mapper.Map<QuotationComparativeDto>(quotationComparative);

            return new Response<QuotationComparativeDto>(quotationComparativeDto, "Returning value");
        }
        
        public async Task<Response<QuotationComparativeDto>> UpdateAsync(CreateQuotationComparativeDto entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitQuotationComparative(entity);
            }
            else
            {
                return await this.UpdateQuotationComparative(entity);
            }
        }
        
        private async Task<Response<QuotationComparativeDto>> SubmitQuotationComparative(CreateQuotationComparativeDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveQuotationComparative(entity);
            }
            else
            {
                return await this.UpdateQuotationComparative(entity);
            }
        }
        
        private async Task<Response<QuotationComparativeDto>> UpdateQuotationComparative(CreateQuotationComparativeDto entity)
        {
            if (entity.QuotationComparativeLines.Count() == 0)
                return new Response<QuotationComparativeDto>("Lines are required");

            var specification = new QuotationComparativeSpecs(true);
            var quotationComparative = await _unitOfWork.QuotationComparative.GetById((int)entity.Id, specification);

            if (quotationComparative == null)
                return new Response<QuotationComparativeDto>("Not found");

            if (quotationComparative.State == DocumentStatus.Submitted)
                return new Response<QuotationComparativeDto>("Only draft document can be edited");

            if (entity.isSubmit == true)
            {
                quotationComparative.setStatus(DocumentStatus.Submitted);
            }
            else
            {
                quotationComparative.setStatus(DocumentStatus.Draft);
            }
            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateQuotationComparativeDto, QuotationComparativeMaster>(entity, quotationComparative);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<QuotationComparativeDto>(_mapper.Map<QuotationComparativeDto>(quotationComparative), "Updated successfully");
        }
        
        private async Task<Response<QuotationComparativeDto>> SaveQuotationComparative(CreateQuotationComparativeDto entity)
        {

            if (entity.QuotationComparativeLines.Count() == 0)
                return new Response<QuotationComparativeDto>("Lines are Required");


            var quotationComparative = _mapper.Map<QuotationComparativeMaster>(entity);

            //Setting status

            if (entity.isSubmit == true)
            {
                quotationComparative.setStatus(DocumentStatus.Submitted);
            }
            else
            {
                quotationComparative.setStatus(DocumentStatus.Draft);
            }
           
            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.QuotationComparative.Add(quotationComparative);
           
            await _unitOfWork.SaveAsync();
           
            //For creating docNo
            quotationComparative.CreateDocNo();
            await _unitOfWork.SaveAsync();

            //Commiting the transaction 
            _unitOfWork.Commit();

            return new Response<QuotationComparativeDto>(_mapper.Map<QuotationComparativeDto>(result), "Created successfully");
        }

    }
}
