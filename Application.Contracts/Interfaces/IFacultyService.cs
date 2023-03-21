using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IFacultyService : ICrudService<CreateFacultyDto, FacultyDto, int, TransactionFormFilter>
    {
        Task<Response<List<FacultyDto>>> GetDropDown();
    }
}