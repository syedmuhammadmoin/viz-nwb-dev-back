using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IPaymentService : ICrudService<CreatePaymentDto, PaymentDto, int, PaginationFilter> 
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
        Task<PaginationResponse<List<PaymentDto>>> GetAllAsync(PaginationFilter filter, PaymentType paymentType, DocType docType);
        Task<Response<PaymentDto>> GetByIdAsync(int id, PaymentType paymentType, DocType docType);
        Response<List<UnReconStmtDto>> GetBankUnreconciledPayments(Guid id);
        Task<Response<bool>> CreatePayrollPaymentProcess(CreatePayrollPaymentDto data);
        //Response<List<PaymentDto>> GetPaymentByDept(DeptFilter data);

    }
}
