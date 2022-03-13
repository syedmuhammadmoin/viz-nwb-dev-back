using Application.Contracts.DTOs;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IBankReconService
    {
        Task<Response<int>> CreateAsync(CreateBankReconDto entity);
    }
}
