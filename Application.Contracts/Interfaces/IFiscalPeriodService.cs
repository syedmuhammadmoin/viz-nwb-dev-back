using Application.Contracts.DTOs.FiscalPeriod;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IFiscalPeriodService : ICrudService<CreateFiscalPeriodDto , FiscalPeriodDto,int ,TransactionFormFilter>
    {
        Task<Response<bool>> DeleteBulkRecords(List<int> ids);
    }
}
