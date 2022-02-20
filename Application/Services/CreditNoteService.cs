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
    public class CreditNoteService : ICreditNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreditNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<CreditNoteDto>> CreateAsync(CreateCreditNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitCRN(entity);
            }
            else
            {
                return await this.SaveCRN(entity, DocumentStatus.Draft);
            }
        }

        public async Task<PaginationResponse<List<CreditNoteDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new CreditNoteSpecs(filter);
            var crns = await _unitOfWork.CreditNote.GetAll(specification);

            if (crns.Count() == 0)
                return new PaginationResponse<List<CreditNoteDto>>("List is empty");

            var totalRecords = await _unitOfWork.CreditNote.TotalRecord();

            return new PaginationResponse<List<CreditNoteDto>>(_mapper.Map<List<CreditNoteDto>>(crns),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CreditNoteDto>> GetByIdAsync(int id)
        {
            var specification = new CreditNoteSpecs(false);
            var crn = await _unitOfWork.CreditNote.GetById(id, specification);
            if (crn == null)
                return new Response<CreditNoteDto>("Not found");

            return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(crn), "Returning value");
        }

        public async Task<Response<CreditNoteDto>> UpdateAsync(CreateCreditNoteDto entity)
        {
            if (entity.isSubmit)
            {
                return await this.SubmitCRN(entity);
            }
            else
            {
                return await this.UpdateCRN(entity, DocumentStatus.Draft);
            }
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private methods
        private async Task<Response<CreditNoteDto>> SubmitCRN(CreateCreditNoteDto entity)
        {
            if (entity.Id == null)
            {
                return await this.SaveCRN(entity, DocumentStatus.Submitted);
            }
            else
            {
                return await this.UpdateCRN(entity, DocumentStatus.Submitted);
            }
        }
        private async Task<Response<CreditNoteDto>> SaveCRN(CreateCreditNoteDto entity, DocumentStatus status)
        {
            if (entity.CreditNoteLines.Count() == 0)
                return new Response<CreditNoteDto>("Lines are required");

            var crn = _mapper.Map<CreditNoteMaster>(entity);

            //Setting status
            crn.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //Saving in table
                var result = await _unitOfWork.CreditNote.Add(crn);
                await _unitOfWork.SaveAsync();

                //For creating docNo
                crn.CreateDocNo();
                await _unitOfWork.SaveAsync();

                //Commiting the transaction 
                _unitOfWork.Commit();

                //returning response
                return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<CreditNoteDto>(ex.Message);
            }
        }
        private async Task<Response<CreditNoteDto>> UpdateCRN(CreateCreditNoteDto entity, DocumentStatus status)
        {
            if (entity.CreditNoteLines.Count() == 0)
                return new Response<CreditNoteDto>("Lines are required");

            var specification = new CreditNoteSpecs(true);
            var crn = await _unitOfWork.CreditNote.GetById((int)entity.Id, specification);

            if (crn == null)
                return new Response<CreditNoteDto>("Not found");

            if (crn.Status == DocumentStatus.Submitted)
                return new Response<CreditNoteDto>("CreditNote already submitted");

            crn.setStatus(status);

            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateCreditNoteDto, CreditNoteMaster>(entity, crn);

                await _unitOfWork.SaveAsync();

                //Commiting the transaction
                _unitOfWork.Commit();

                //returning response
                return new Response<CreditNoteDto>(_mapper.Map<CreditNoteDto>(crn), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<CreditNoteDto>(ex.Message);
            }
        }

    }
}
