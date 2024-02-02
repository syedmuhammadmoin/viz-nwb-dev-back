using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IPayrollTransactionService : ICrudService<CreatePayrollTransactionDto[], UpdatePayrollTransactionDto, PayrollTransactionDto, int, TransactionFormFilter>
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
        Task<Response<bool>> ProcessForEdit(int[] id);
        Task<Response<bool>> ProcessForApproval(CreateApprovalProcessDto data);
        Task<Response<List<PayrollTransactionDto>>> GetEmployeesByDept(DeptFilter data);
        Response<List<PayrollTransactionDto>> GetPayrollTransactionByDept(DeptFilter data);
        Response<List<PayrollTransactionDto>> GetPayrollReport(PayrollFilter filter);
        Response<List<PayrollCampusTransactionDto>> GetPayrollCampusGroupReport(PayrollCampusReportFilter filter);
        Response<DataTable> GetPayrollDetailReport(PayrollDetailFilter filter);
        Task<Response<DataTable>> GetPayrollReport(DeptFilter data);
        Task<MemoryStream> ExportPayrollDetailedReport(PayrollDetailFilter filter);
        Response<PayrollExecutiveReportDto> GetPayrollExecutiveReport(PayrollExecutiveReportFilter filter);
        Response<List<BankAdviceReportDto>> GetBankAdviceReportReport(BankAdviceReportFilter filter);
    }
}
