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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GoodsReturnNoteService : IGoodsReturnNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GoodsReturnNoteService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<Response<bool>> CheckWorkFlow(ApprovalDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<GoodsReturnNoteDto>> CreateAsync(CreateGoodsReturnNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitGoodsReturnNote(entity);
            }
            else
            {
                return await this.SaveGoodsReturnNote(entity, 1);
            }
        }

        public async Task<PaginationResponse<List<GoodsReturnNoteDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new GoodsReturnNoteSpecs(filter);
            var goodsReturnNotes = await _unitOfWork.GoodsReturnNote.GetAll(specification);

            if (goodsReturnNotes.Count() == 0)
                return new PaginationResponse<List<GoodsReturnNoteDto>>(_mapper.Map<List<GoodsReturnNoteDto>>(goodsReturnNotes), "List is empty");

            var totalRecords = await _unitOfWork.GoodsReturnNote.TotalRecord();

            return new PaginationResponse<List<GoodsReturnNoteDto>>(_mapper.Map<List<GoodsReturnNoteDto>>(goodsReturnNotes),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<GoodsReturnNoteDto>> GetByIdAsync(int id)
        {
            var specification = new GoodsReturnNoteSpecs(false);
            var goodsReturnNote = await _unitOfWork.GoodsReturnNote.GetById(id, specification);
            if (goodsReturnNote == null)
                return new Response<GoodsReturnNoteDto>("Not found");

            var goodsReturnNoteDto = _mapper.Map<GoodsReturnNoteDto>(goodsReturnNote);

            goodsReturnNoteDto.IsAllowedRole = false;
            var workflow = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GoodsReturnNote)).FirstOrDefault();


            if (workflow != null)
            {
                var transition = workflow.WorkflowTransitions
                    .FirstOrDefault(x => (x.CurrentStatusId == goodsReturnNoteDto.StatusId));

                if (transition != null)
                {
                    var currentUserRoles = new GetUser(this._httpContextAccessor).GetCurrentUserRoles();
                    foreach (var role in currentUserRoles)
                    {
                        if (transition.AllowedRole.Name == role)
                        {
                            goodsReturnNoteDto.IsAllowedRole = true;
                        }
                    }
                }
            }
            return new Response<GoodsReturnNoteDto>(goodsReturnNoteDto, "Returning value");

        }

        public async Task<Response<GoodsReturnNoteDto>> UpdateAsync(CreateGoodsReturnNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitGoodsReturnNote(entity);
            }
            else
            {
                return await this.UpdateGoodsReturnNote(entity, 1);
            }
        }
        
        //Privte Methods for GoodsReturnNote

        private async Task<Response<GoodsReturnNoteDto>> SubmitGoodsReturnNote(CreateGoodsReturnNoteDto entity)
        {
            var checkingActiveWorkFlows = _unitOfWork.WorkFlow.Find(new WorkFlowSpecs(DocType.GoodsReturnNote)).FirstOrDefault();

            if (checkingActiveWorkFlows == null)
            {
                return new Response<GoodsReturnNoteDto>("No workflow found for GoodsReturnNote");
            }

            if (entity.Id == null)
            {
                return await this.SaveGoodsReturnNote(entity, 6);
            }
            else
            {
                return await this.UpdateGoodsReturnNote(entity, 6);
            }
        }

        private async Task<Response<GoodsReturnNoteDto>> SaveGoodsReturnNote(CreateGoodsReturnNoteDto entity, int status)
        {
            if (entity.GoodsReturnNoteLines.Count() == 0)
                return new Response<GoodsReturnNoteDto>("Lines are required");

            foreach (var goodsReturnNoteLine in entity.GoodsReturnNoteLines)
            {
                //Getting Unreconciled GoodsReturnNote lines
                var getGoodsReturnNoteLine = _unitOfWork.GRN
                    .FindLines(new GRNLinesSpecs(goodsReturnNoteLine.ItemId, goodsReturnNoteLine.WarehouseId, entity.GRNId))
                    .FirstOrDefault();
                if (getGoodsReturnNoteLine == null)
                    return new Response<GoodsReturnNoteDto>("No GoodsReturnNote line found for reconciliaiton");

                var checkValidation = CheckValidation(entity.GRNId, getGoodsReturnNoteLine, _mapper.Map<GoodsReturnNoteLines>(goodsReturnNoteLine));
                if (!checkValidation.IsSuccess)
                    return new Response<GoodsReturnNoteDto>(checkValidation.Message);
            }

            //Checking duplicate Lines if any
            var duplicates = entity.GoodsReturnNoteLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<GoodsReturnNoteDto>("Duplicate Lines found");

            var goodsReturnNote = _mapper.Map<GoodsReturnNoteMaster>(entity);

            //Setting status
            goodsReturnNote.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.GoodsReturnNote.Add(goodsReturnNote);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                goodsReturnNote.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<GoodsReturnNoteDto>(_mapper.Map<GoodsReturnNoteDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<GoodsReturnNoteDto>(ex.Message);
            }
        }

        private async Task<Response<GoodsReturnNoteDto>> UpdateGoodsReturnNote(CreateGoodsReturnNoteDto entity, int status)
        {
            //setting GoodsReturnNoteId
            var getGRN = await _unitOfWork.GoodsReturnNote.GetById(entity.GRNId, new GoodsReturnNoteSpecs(false));

            if (getGRN == null)
                return new Response<GoodsReturnNoteDto>("GRN not found");

            if (entity.GoodsReturnNoteLines.Count() == 0)
                return new Response<GoodsReturnNoteDto>("Lines are required");

            foreach (var goodsReturnNoteLine in entity.GoodsReturnNoteLines)
            {
                //Getting Unreconciled GoodsReturnNote lines
                var getGoodsReturnNoteLine = _unitOfWork.GRN
                    .FindLines(new GRNLinesSpecs(goodsReturnNoteLine.ItemId, goodsReturnNoteLine.WarehouseId, entity.GRNId))
                    .FirstOrDefault();
                if (getGoodsReturnNoteLine == null)
                    return new Response<GoodsReturnNoteDto>("No GoodsReturnNote line found for reconciliaiton");

                var checkValidation = CheckValidation(entity.GRNId, getGoodsReturnNoteLine, _mapper.Map<GoodsReturnNoteLines>(goodsReturnNoteLine));
                if (!checkValidation.IsSuccess)
                    return new Response<GoodsReturnNoteDto>(checkValidation.Message);
            }

            //Checking duplicate Lines if any
            var duplicates = entity.GoodsReturnNoteLines.GroupBy(x => new { x.ItemId, x.WarehouseId })
             .Where(g => g.Count() > 1)
             .Select(y => y.Key)
             .ToList();

            if (duplicates.Any())
                return new Response<GoodsReturnNoteDto>("Duplicate Lines found");

            var specification = new GoodsReturnNoteSpecs(true);
            var gRN = await _unitOfWork.GoodsReturnNote.GetById((int)entity.Id, specification);

            if (gRN == null)
                return new Response<GoodsReturnNoteDto>("Not found");

            if (gRN.StatusId != 1 && gRN.StatusId != 2)
                return new Response<GoodsReturnNoteDto>("Only draft document can be edited");
            //Setting GoodsReturnNoteId
            gRN.setGRNId(getGRN.Id);

            //Setting status
            gRN.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateGoodsReturnNoteDto, GoodsReturnNoteMaster>(entity, gRN);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<GoodsReturnNoteDto>(_mapper.Map<GoodsReturnNoteDto>(gRN), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<GoodsReturnNoteDto>(ex.Message);
            }
        }

        public Response<bool> CheckValidation(int grnId, GRNLines grnLine, GoodsReturnNoteLines goodsReturnNoteLines)
        {
            // Checking if given amount is greater than unreconciled document amount
            var reconciledGRNQty = _unitOfWork.GRNToGoodsReturnNoteReconcile
                .Find(new GRNToGoodsReturnNoteReconcileSpecs(grnId, grnLine.Id, grnLine.ItemId, grnLine.WarehouseId))
                .Sum(p => p.Quantity);

            var unreconciledGRNQty = grnLine.Quantity - reconciledGRNQty;

            if (grnLine.Quantity > unreconciledGRNQty)
                return new Response<bool>("Enter quantity is greater than pending quantity");

            return new Response<bool>(true, "No validation error found");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
