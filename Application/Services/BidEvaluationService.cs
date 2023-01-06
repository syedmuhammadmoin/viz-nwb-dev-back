using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BidEvaluationService : IBidEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BidEvaluationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<PaginationResponse<List<BidEvaluationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var OpeningDate = new List<DateTime?>();
            var states = new List<DocumentStatus?>();
            if (filter.DocDate != null)
            {
                OpeningDate.Add(filter.DocDate);
            }
            if (filter.State != null)
            {
                states.Add(filter.State);
            }

            var bidEvaluation = await _unitOfWork.BidEvaluation.GetAll(new BidEvaluationSpecs(OpeningDate, states, filter, false));

            if (bidEvaluation.Count() == 0)
                return new PaginationResponse<List<BidEvaluationDto>>(_mapper.Map<List<BidEvaluationDto>>(bidEvaluation), "List is empty");

            var totalRecords = await _unitOfWork.BidEvaluation.TotalRecord(new BidEvaluationSpecs(OpeningDate, states, filter, true));

            return new PaginationResponse<List<BidEvaluationDto>>(_mapper.Map<List<BidEvaluationDto>>(bidEvaluation),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BidEvaluationDto>> GetByIdAsync(int id)
        {
            var bidEvaluation = await _unitOfWork.BidEvaluation.GetById(id, new BidEvaluationSpecs());
            if (bidEvaluation == null)
                return new Response<BidEvaluationDto>("Not found");

            var bidEvaluationDto = _mapper.Map<BidEvaluationDto>(bidEvaluation);


            return new Response<BidEvaluationDto>(bidEvaluationDto, "Returning value");
        }

        public async Task<Response<BidEvaluationDto>> CreateAsync(CreateBidEvaluationDto entity)
        {
            if (entity.BidEvaluationLines.Count() == 0)
                return new Response<BidEvaluationDto>("Lines are required");

            var bidEvaluation = _mapper.Map<BidEvaluationMaster>(entity);

            if ((bool)entity.isSubmit)
            {
                bidEvaluation.setStatus(DocumentStatus.Submitted);
            }
            else
            {
                bidEvaluation.setStatus(DocumentStatus.Draft);
            }
            _unitOfWork.CreateTransaction();
            //Saving in table
            var result = await _unitOfWork.BidEvaluation.Add(bidEvaluation);
            await _unitOfWork.SaveAsync();
            
            bidEvaluation.CreateDocNo();
            await _unitOfWork.SaveAsync();
            
            //Commiting the transaction 
            _unitOfWork.Commit();

            //returning response
            return new Response<BidEvaluationDto>(_mapper.Map<BidEvaluationDto>(result), "Created successfully");
        }

        public async Task<Response<BidEvaluationDto>> UpdateAsync(CreateBidEvaluationDto entity)
        {
            if (entity.BidEvaluationLines.Count() == 0)
                return new Response<BidEvaluationDto>("Lines are required");

            var bidEvaluation = await _unitOfWork.BidEvaluation.GetById((int)entity.Id, new BidEvaluationSpecs());
            if (bidEvaluation == null)
                return new Response<BidEvaluationDto>("Not found");
            
            if (bidEvaluation.State != DocumentStatus.Draft)
                return new Response<BidEvaluationDto>("Only draft document can be edited");

            if ((bool)entity.isSubmit)
            {
                bidEvaluation.setStatus(DocumentStatus.Submitted);
            }
            else
            {
                bidEvaluation.setStatus(DocumentStatus.Draft);
            }
            //For updating data
            _mapper.Map<CreateBidEvaluationDto, BidEvaluationMaster>(entity, bidEvaluation);
            
            _unitOfWork.CreateTransaction();
            
            await _unitOfWork.SaveAsync();
            
            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<BidEvaluationDto>(_mapper.Map<BidEvaluationDto>(bidEvaluation), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
