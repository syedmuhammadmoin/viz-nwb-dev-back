using Application.Contracts.DTOs;
using Application.Contracts.DTOs.BidEvaluation;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BidEvaluationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BidEvaluationDto>> CreateAsync(CreateBidEvaluationDtos entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitBidEvaluation(entity);
            }
            else
            {
                return await this.SaveBidEvaluation(entity, DocumentStatus.Draft);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<List<BidEvaluationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BidEvaluationDto>> GetByIdAsync(int id)
        {
            var specification = new BidEvaluationSpecs(false);
            var bidEvaluation = await _unitOfWork.BidEvaluation.GetById(id, specification);
            if (bidEvaluation == null)
                return new Response<BidEvaluationDto>("Not found");

            var bidEvaluationDto = _mapper.Map<BidEvaluationDto>(bidEvaluation);
           
          
            return new Response<BidEvaluationDto>(bidEvaluationDto, "Returning value");
        }

        public async Task<Response<BidEvaluationDto>> UpdateAsync(CreateBidEvaluationDtos entity)
        {
            if ((bool)entity.isSubmit)
            {
                return await this.SubmitBidEvaluation(entity);
            }
            else
            {
                return await this.UpdateBidEvaluation(entity, DocumentStatus.Draft);
            }
        }

        private async Task<Response<BidEvaluationDto>> SubmitBidEvaluation(CreateBidEvaluationDtos entity )
        {
            if (entity.Id == null)
            {
                return await this.SaveBidEvaluation(entity, DocumentStatus.Submitted);
            }
            else
            {
                return await this.UpdateBidEvaluation(entity, DocumentStatus.Submitted);
            }
        }
        private async Task<Response<BidEvaluationDto>> SaveBidEvaluation(CreateBidEvaluationDtos entity , DocumentStatus status)
        {
            if (entity.BidEvaluationLines.Count() == 0)
                return new Response<BidEvaluationDto>("Lines are required");

            var bidEvaluation = _mapper.Map<BidEvaluationMaster>(entity);
            _unitOfWork.CreateTransaction();

            bidEvaluation.setStatus(status);

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
        private async Task<Response<BidEvaluationDto>> UpdateBidEvaluation(CreateBidEvaluationDtos entity, DocumentStatus status)
        {
            if (entity.BidEvaluationLines.Count() == 0)
                return new Response<BidEvaluationDto>("Lines are required");

            var specification = new BidEvaluationSpecs(true);
            var bidEvaluation = await _unitOfWork.BidEvaluation.GetById((int)entity.Id, specification);

            if (bidEvaluation == null)
                return new Response<BidEvaluationDto>("Not found");

            bidEvaluation.setStatus(status);

            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map<CreateBidEvaluationDtos, BidEvaluationMaster>(entity, bidEvaluation);

            await _unitOfWork.SaveAsync();

            //Commiting the transaction
            _unitOfWork.Commit();

            //returning response
            return new Response<BidEvaluationDto>(_mapper.Map<BidEvaluationDto>(bidEvaluation), "Created successfully");

        }
    }
}
