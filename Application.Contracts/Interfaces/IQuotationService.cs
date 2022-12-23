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
    public interface IQuotationService : ICrudService<CreateQuotationDto, QuotationDto, int, TransactionFormFilter>
    {
        Task<Response<List<QuotationDto>>> GetQoutationByRequisitionId(int requisitionId);
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
    }
}
