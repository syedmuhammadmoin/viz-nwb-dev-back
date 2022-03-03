using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Infrastructure.Specifications;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Constants;

namespace Application.Services
{
    public class BankStmtService : IBankStmtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankStmtService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<BankStmtDto>> CreateAsync(CreateBankStmtDto entity)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var bankStmt = _mapper.Map<BankStmtMaster>(entity);

                bankStmt.setStatus(ReconStatus.Unreconciled);

                await _unitOfWork.Bankstatement.Add(bankStmt);
                await _unitOfWork.SaveAsync();

                _unitOfWork.Commit();
                return new Response<BankStmtDto>(_mapper.Map<BankStmtDto>(bankStmt), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<BankStmtDto>(ex.Message);
            }
        }

        public async Task<PaginationResponse<List<BankStmtDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new BankStmtSpecs(filter);
            var bankStmts = await _unitOfWork.Bankstatement.GetAll(specification);

            if (!bankStmts.Any())
                return new PaginationResponse<List<BankStmtDto>>("List is empty");

            var totalRecords = await _unitOfWork.Bankstatement.TotalRecord();

            return new PaginationResponse<List<BankStmtDto>>(_mapper.Map<List<BankStmtDto>>(bankStmts),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<BankStmtDto>> GetByIdAsync(int id)
        {
            var specification = new BankStmtSpecs();
            var bankStmt = await _unitOfWork.Bankstatement.GetById(id, specification);
            if (bankStmt == null)
                return new Response<BankStmtDto>("Not found");

            return new Response<BankStmtDto>(_mapper.Map<BankStmtDto>(bankStmt), "Returning value");

        }

        public async Task<Response<BankStmtDto>> UpdateAsync(CreateBankStmtDto entity)
        {
            if (entity.BankStmtLines.Count == 0)
                return new Response<BankStmtDto>("Lines are required");

            var specification = new BankStmtSpecs();
            var bankStmt = await _unitOfWork.Bankstatement.GetById((int)entity.Id, specification);
            
            if (bankStmt.BankReconStatus!= ReconStatus.Unreconciled)
                return new Response<BankStmtDto>("Statement lines already reconciled");
            
            _unitOfWork.CreateTransaction();
            try
            {
                //For updating data
                _mapper.Map<CreateBankStmtDto, BankStmtMaster>(entity, bankStmt);

                _unitOfWork.Commit();

                //returning response
                return new Response<BankStmtDto>(_mapper.Map<BankStmtDto>(bankStmt), "Updated successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<BankStmtDto>(ex.Message);
            }
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
