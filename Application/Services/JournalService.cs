using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
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
    public class JournalService : IJournalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JournalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<List<JournalDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Journals.GetAll(new JournalSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<JournalDto>>(_mapper.Map<List<JournalDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Journals.TotalRecord(new JournalSpecs(filter, true));
            return new PaginationResponse<List<JournalDto>>(_mapper.Map<List<JournalDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }
        public async Task<Response<JournalDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Journals.GetById(id, new JournalSpecs());
            if (result == null)
                return new Response<JournalDto>("Not found");

            return new Response<JournalDto>(_mapper.Map<JournalDto>(result), "Returning value");
        }
        public async Task<Response<List<JournalDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Journals.GetAll();
            if (!result.Any())
                return new Response<List<JournalDto>>(null, "List is empty");

            return new Response<List<JournalDto>>(_mapper.Map<List<JournalDto>>(result), "Returning List");
        }

        public async Task<Response<JournalDto>> CreateAsync(CreateJournalDto entity)
        {
            var result = await _unitOfWork.Journals.Add(_mapper.Map<Journal>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<JournalDto>(_mapper.Map<JournalDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<JournalDto>> UpdateAsync(CreateJournalDto entity)
        {
            var result = await _unitOfWork.Journals.GetById((int)entity.Id);
            if (result == null)
                return new Response<JournalDto>("Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<JournalDto>(_mapper.Map<JournalDto>(result), "Updated successfully");
        }
    }
}
