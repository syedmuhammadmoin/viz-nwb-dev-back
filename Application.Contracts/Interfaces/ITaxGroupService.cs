using Application.Contracts.DTOs.TaxGroup;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface ITaxGroupService : ICrudService<CreateTaxGroupDto,TaxGroupDto,int ,TransactionFormFilter>
    {
        Task<Response<bool>> DeleteTaxGroups(List<int> ids);

        Task<Response<List<TaxGroupDto>>> GetTaxDropdown();
    }
}
