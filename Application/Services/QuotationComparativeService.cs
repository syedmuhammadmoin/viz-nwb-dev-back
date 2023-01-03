﻿using Application.Contracts.DTOs;
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
                return new PaginationResponse<List<QuotationComparativeDto>>(null, "List is empty");

            var totalRecords = await _unitOfWork.QuotationComparative.TotalRecord(new QuotationComparativeSpecs(DocDate, states, filter, true));

            return new PaginationResponse<List<QuotationComparativeDto>>(_mapper.Map<List<QuotationComparativeDto>>(quotationComparative),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<QuotationComparativeDto>> GetByIdAsync(int id)
        {
            var quotationComparative = await _unitOfWork.QuotationComparative.GetById(id, new QuotationComparativeSpecs());
            if (quotationComparative == null)
                return new Response<QuotationComparativeDto>("Not found");

            var quotationComparativeDto = _mapper.Map<QuotationComparativeDto>(quotationComparative);
            var getQuotations = await _unitOfWork.Quotation.GetAll(new QuotationSpecs(true, quotationComparativeDto.Id));
            quotationComparativeDto.Quotations = _mapper.Map<List<QuotationDto>>(getQuotations);
            
            return new Response<QuotationComparativeDto>(quotationComparativeDto, "Returning value");
        }

        public async Task<Response<QuotationComparativeDto>> CreateAsync(CreateQuotationComparativeDto entity)
        {
            if (entity.QuotationComparativeLines.Count() == 0)
                return new Response<QuotationComparativeDto>("Lines are Required");

            if(entity.QuotationComparativeLines.Count() < 3)
                return new Response<QuotationComparativeDto>("Minimum 3 lines are Required");

            var duplicates = entity.QuotationComparativeLines.GroupBy(x => new { x.QuotationId })
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

            if (duplicates.Any())
                return new Response<QuotationComparativeDto>("Duplicate Lines found");

            var quotationComparative = new QuotationComparativeMaster(
                entity.QuotationComparativeDate,
                (int)entity.RequisitionId,
                entity.isSubmit == true ? DocumentStatus.Submitted : DocumentStatus.Draft);

            _unitOfWork.CreateTransaction();

            //Saving in table
            var result = await _unitOfWork.QuotationComparative.Add(quotationComparative);
            await _unitOfWork.SaveAsync();

            //For creating docNo
            quotationComparative.CreateDocNo();
            await _unitOfWork.SaveAsync();

            foreach (var line in entity.QuotationComparativeLines)
            {
                if (line.isSelected)
                {
                    var quotation = await _unitOfWork.Quotation.GetById(line.QuotationId);
                    quotation.UpdateQuotationComparativeMasterId(quotationComparative.Id);
                    await _unitOfWork.SaveAsync();
                }
            }

            //Commiting the transaction 
            _unitOfWork.Commit();
            return new Response<QuotationComparativeDto>(_mapper.Map<QuotationComparativeDto>(result), "Created successfully");
        }
        
        public async Task<Response<QuotationComparativeDto>> UpdateAsync(CreateQuotationComparativeDto entity)
        {
            if (entity.QuotationComparativeLines.Count() == 0)
                return new Response<QuotationComparativeDto>("Lines are required");

            var quotationComparative = await _unitOfWork.QuotationComparative.GetById((int)entity.Id, new QuotationComparativeSpecs());
            if (quotationComparative == null)
                return new Response<QuotationComparativeDto>("Not found");

            if (quotationComparative.Status == DocumentStatus.Submitted)
                return new Response<QuotationComparativeDto>("Only draft document can be edited");

            quotationComparative.Update(
                entity.QuotationComparativeDate,
                (int)entity.RequisitionId,
                entity.isSubmit == true ? DocumentStatus.Submitted : DocumentStatus.Draft);

            _unitOfWork.CreateTransaction();
            await _unitOfWork.SaveAsync();
            foreach (var line in entity.QuotationComparativeLines)
            {
                var quotation = await _unitOfWork.Quotation.GetById(line.QuotationId);
                if (line.isSelected)
                {
                    quotation.UpdateQuotationComparativeMasterId(quotationComparative.Id);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    quotation.UpdateQuotationComparativeMasterId(null);
                    await _unitOfWork.SaveAsync();
                }
            }
            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<QuotationComparativeDto>(_mapper.Map<QuotationComparativeDto>(quotationComparative), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<AwardedVendorDto>> AwardedVendorQuotation(int quotationCompId, AwardedVendorDto entity)
        {
            var quotationComparative = await _unitOfWork.QuotationComparative.GetById(quotationCompId, new QuotationComparativeSpecs());
            if (quotationComparative == null)
                return new Response<AwardedVendorDto>("Not found");

            if (quotationComparative.Status == DocumentStatus.Draft)
                return new Response<AwardedVendorDto>("Only Submitted document can be Awarded");
            
            var quotations = await _unitOfWork.Quotation.GetById(entity.QuotationId, new QuotationSpecs(false)); 
          
            if (quotations == null)
                return new Response<AwardedVendorDto>("Not found");
            if (quotationComparative.AwardedVendor != null)
                return new Response<AwardedVendorDto>("This Vendor is already Awarded");

            var awardedVendorDto = _mapper.Map<AwardedVendorDto>(quotationComparative);

            awardedVendorDto.checkBoxSelection = false;
           
            quotations.UpdateAwardedVendor(
                quotations.Id
                );
            
            quotationComparative.UpdateAwardedVendor(
             entity.AwardedVendor,
             entity.Remarks
                );

            _unitOfWork.CreateTransaction();
            await _unitOfWork.SaveAsync();
            _unitOfWork.Commit();
            awardedVendorDto.AwardedVendor = quotationComparative.AwardedVendor;
            awardedVendorDto.Remarks = quotationComparative.Remarks;
            awardedVendorDto.QuotationId = quotations.Id;
            return new Response<AwardedVendorDto>(_mapper.Map<AwardedVendorDto>(awardedVendorDto), "Updated successfully");
        }
    }
}
