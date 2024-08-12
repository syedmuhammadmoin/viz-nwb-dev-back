﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface ILevel4Service : ICrudService<CreateLevel4Dto, Level4Dto, string, PaginationFilter>
    {
        Task<Response<List<Level4Dto>>> GetLevel4DropDown();
        Task<Response<List<Level4Dto>>> GetBudgetAccounts();
        Task<Response<List<Level4Dto>>> GetPayableAccounts();
        Task<Response<List<Level4Dto>>> GetReceivableAccounts();
        Task<Response<List<Level4Dto>>> GetAllOtherAccounts();
        Task<Response<List<Level4Dto>>> GetNonCurrentAssetAccounts();
        Task<Response<List<Level4Dto>>> GetNonCurrentLiabilitiesAccounts();
        Task<Response<List<Level4Dto>>> GetExpenseAccounts();
    }
}
