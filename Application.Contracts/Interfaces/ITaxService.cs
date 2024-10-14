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
    public interface ITaxService : ICrudService<CreateTaxDto, TaxDto, int, TransactionFormFilter>
    {
        Task<Response<bool>> DeleteTaxes(List<int> ids);
        Task<Response<List<TaxDto>>> GetTaxesWithIds(List<int> ids);
        Task<Response<bool>> InActiveTax(int id, bool status);
    }
}
