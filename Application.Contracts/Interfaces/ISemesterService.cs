using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface ISemesterService : ICrudService<SemesterDto, SemesterDto, int, TransactionFormFilter>
    {
        Task<Response<List<SemesterDto>>> GetDropDown();
    }
}
