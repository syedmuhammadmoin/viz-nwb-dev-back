using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IDepreciationService : ICrudService<CreateDepreciationDto, DepreciationDto, int, TransactionFormFilter>
    {
        Task<Response<List<DepreciationDto>>> GetDepreciationDown();
    }
}
