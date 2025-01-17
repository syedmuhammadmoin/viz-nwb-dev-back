﻿using Application.Contracts.DTOs;
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
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

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

        public async Task<Response<BankStmtDto>> CreateAsync(CreateBankStmtDto entity, IFormFile file)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var bankStmtLinesArray = new List<BankStmtLines>();

                if (file != null)
                {
                    entity.BankStmtLines = await ImportStmtLines(file);

                    if (entity.BankStmtLines.Count() == 0)
                        return new Response<BankStmtDto>("Lines are required");

                    foreach (var line in entity.BankStmtLines)
                    {
                        if (line.Credit < 0 || line.Debit < 0)
                        {
                            return new Response<BankStmtDto>("Credit & Debit amount should be a postive value");
                        }

                        if (line.StmtDate.Date > DateTime.UtcNow)
                        {
                            return new Response<BankStmtDto>("Future date statment can not be created ");
                        }
                    }
                }

                if (entity.BankStmtLines.Count() == 0)
                    return new Response<BankStmtDto>("Lines are required, Upload a file or click on reset for Lines");

                foreach (var line in entity.BankStmtLines)
                {
                    if (line.Credit == 0 && line.Debit == 0)
                    {
                        return new Response<BankStmtDto>("Amount can't be saved with zero value");
                    }

                    if (line.Credit == line.Debit || line.Debit > 0 && line.Credit > 0)
                    {
                        return new Response<BankStmtDto>("Only one entry should be entered at a time");
                    }
                }

                var bankStmt = _mapper.Map<BankStmtMaster>(entity);
                await _unitOfWork.Bankstatement.Add(bankStmt);
                await _unitOfWork.SaveAsync();

                _unitOfWork.Commit();
                return new Response<BankStmtDto>(_mapper.Map<BankStmtDto>(bankStmt), "Created successfully");
            }
            catch (FormatException)
            {
                _unitOfWork.Rollback();
                return new Response<BankStmtDto>("Lines are in wrong format");
            }
            catch (InvalidCastException)
            {
                _unitOfWork.Rollback();
                return new Response<BankStmtDto>("Date Format should be DD/MM/YYYY");
            }
            catch (NullReferenceException)
            {
                _unitOfWork.Rollback();
                return new Response<BankStmtDto>("All fields are required in spreadsheet");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<BankStmtDto>(ex.Message);
            }

        }

        public async Task<PaginationResponse<List<BankStmtDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var bankStmts = await _unitOfWork.Bankstatement.GetAll(new BankStmtSpecs(filter, false)
                );

            if (!bankStmts.Any())
                return new PaginationResponse<List<BankStmtDto>>(_mapper.Map<List<BankStmtDto>>(bankStmts), "List is empty");

            var totalRecords = await _unitOfWork.Bankstatement.TotalRecord(new BankStmtSpecs(filter, true));

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

            foreach (var line in bankStmt.BankStmtLines)
            {
                if (line.BankReconStatus != DocumentStatus.Unreconciled)
                {
                    return new Response<BankStmtDto>("Statement lines already reconciled");
                }
            }

            //For updating data
            _mapper.Map<CreateBankStmtDto, BankStmtMaster>(entity, bankStmt);
            await _unitOfWork.SaveAsync();

            //returning response
            return new Response<BankStmtDto>(_mapper.Map<BankStmtDto>(bankStmt), "Updated successfully");

        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Private methods for importing BankStmtLines
        private async Task<List<CreateBankStmtLinesDto>> ImportStmtLines(IFormFile file)
        {
            var list = new List<CreateBankStmtLinesDto>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var checkDate = ((DateTime)worksheet.Cells[row, 2].Value);

                        list.Add(new CreateBankStmtLinesDto()
                        {
                            Reference = (int)Convert.ToSingle(worksheet.Cells[row, 1].Value),
                            StmtDate = (DateTime)worksheet.Cells[row, 2].Value,
                            Label = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            Debit = Convert.ToDecimal(worksheet.Cells[row, 4].Value),
                            Credit = Convert.ToDecimal(worksheet.Cells[row, 5].Value),
                        });
                    }
                }
            }
            return list;
        }

        public Task<Response<BankStmtDto>> CreateAsync(CreateBankStmtDto entity)
        {
            throw new NotImplementedException();
        }

        public Response<List<UnReconStmtDto>> GetBankUnreconciledStmts(int id)
        {
            List<UnReconStmtDto> unreconciledBankStmtStatus = new List<UnReconStmtDto>();

            var bankStmts = _unitOfWork.BankStmtLines.Find(new BankStmtLinesSpecs(id))
                .Select(i => new UnReconStmtDto()
                {
                    Id = i.Id,
                    DocNo = i.Reference.ToString(),
                    DocDate = i.StmtDate,
                    Label = i.Label,
                    Amount = i.Credit - i.Debit,
                    BankReconStatus = i.BankReconStatus
                })
                .ToList();

            if (bankStmts.Count() == 0)
                return new Response<List<UnReconStmtDto>>(unreconciledBankStmtStatus, "Unreconciled bank statements not found");

            foreach (var e in bankStmts)
            {
                var reconciledPayment = _unitOfWork.BankReconciliation.Find(new BankReconSpecs(e.Id, false)).Sum(a => a.Amount);

                var mapingValueInDTO = new UnReconStmtDto
                {
                    Id = e.Id,
                    DocNo = e.DocNo.ToString(),
                    DocDate = e.DocDate,
                    Amount = e.Amount,
                    Label = e.Label,
                    ReconciledAmount = reconciledPayment,
                    UnreconciledAmount = e.Amount < 0 ? ((e.Amount * -1) - reconciledPayment) * -1 : (e.Amount - reconciledPayment),
                    BankReconStatus = e.BankReconStatus
                };
                unreconciledBankStmtStatus.Add(mapingValueInDTO);
            };
            return new Response<List<UnReconStmtDto>>(unreconciledBankStmtStatus, "Returning bank statements");

        }
    }
}
