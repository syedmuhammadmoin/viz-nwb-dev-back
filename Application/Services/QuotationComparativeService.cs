using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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
        public  Response<List<QuotationDto>> GetRequisitionById(int requisition)
        {
           var specification = new QuotationSpecs(requisition);
            var quotation =  _unitOfWork.Quotation.Find(specification).ToList() ;
            if (quotation == null)
                return new Response<List<QuotationDto>>("Not found");
            var quotationDto =  _mapper.Map<List<QuotationDto>>(quotation);
            return new Response<List<QuotationDto>>(quotationDto, "Returning value");
        }
    }
}
