using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IAcademicDepartmentService : ICrudService<CreateAcademicDepartmentDto, AcademicDepartmentDto, int, TransactionFormFilter>
    {
        Task<Response<List<AcademicDepartmentDto>>> GetDropDown();
    }
}
