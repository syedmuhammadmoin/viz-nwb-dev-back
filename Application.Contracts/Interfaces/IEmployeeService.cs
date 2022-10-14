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
    public interface IEmployeeService : ICrudService<CreateEmployeeDto[], UpdateEmployeeDto, EmployeeDto, int, TransactionFormFilter>
    {
        Task<Response<List<EmployeeDto>>> GetEmployeeDropDown();
        Task<Response<List<EmployeeDropDownPaymentDto>>> GetEmployeeDropDownPayment();
        Response<EmployeeDto> GetEmpByCNIC(string cnic);
    }
}
