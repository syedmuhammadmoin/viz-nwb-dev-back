using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IBankStmtService : ICrudService<CreateBankStmtDto, BankStmtDto, int, TransactionFormFilter>
    {
        Task<Response<BankStmtDto>> CreateAsync(CreateBankStmtDto entity, IFormFile file);
        Response<List<UnReconStmtDto>> GetBankUnreconciledStmts(int id);
    }
}
