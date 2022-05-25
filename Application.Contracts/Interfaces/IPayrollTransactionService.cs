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
    public interface IPayrollTransactionService : ICrudService<CreatePayrollTransactionDto, PayrollTransactionDto, int, PaginationFilter>
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
        Task<Response<bool>> ProcessForCreate(CreateProcessDto data);
        Task<Response<bool>> ProcessForEdit(int[] id);
        Task<Response<bool>> ProcessForApproval(CreateApprovalProcessDto data);
    }
}
