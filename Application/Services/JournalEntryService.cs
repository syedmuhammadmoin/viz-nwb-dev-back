using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JournalEntryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<JournalEntryDto>> CreateAsync(CreateJournalEntryDto entity)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                //List<JournalEntryLines> journalEntryLines = entity.JournalEntryLines
                //    .Select(line => new JournalEntryLines
                //    {
                //        AccountId = line.AccountId,
                //        BusinessPartnerId = line.BusinessPartnerId,
                //        Description = line.Description,
                //        Debit = line.Debit,
                //        Credit = line.Credit,
                //        LocationId = line.LocationId
                //    }).ToList();

                List<JournalEntryLines> journalEntryLines = _mapper.Map<List<JournalEntryLines>>(entity.JournalEntryLines); 

                var jv = _mapper.Map<JournalEntryMaster>(entity);
                jv.JournalEntryLines = journalEntryLines;

                var result = await _unitOfWork.JournalEntry.Add(jv);
                await _unitOfWork.SaveAsync();

                ////Creating doc no..
                //string docNo = "JV-" + String.Format("{0:000}", jv.Id);

                ////For updating docNo
                //jv.DocNo = docNo;
                //await context.SaveChangesAsync();

                _unitOfWork.Commit();


                return new Response<JournalEntryDto>(_mapper.Map<JournalEntryDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<JournalEntryDto>(ex.Message);
            }

        }

        public Task<PaginationResponse<List<JournalEntryDto>>> GetAllAsync(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Response<JournalEntryDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<JournalEntryDto>> UpdateAsync(CreateJournalEntryDto entity)
        {
            throw new NotImplementedException();
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
