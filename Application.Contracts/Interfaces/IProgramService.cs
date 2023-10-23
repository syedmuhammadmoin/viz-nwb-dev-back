using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IProgramService : ICrudService<CreateProgramDto, ProgramDto, int, TransactionFormFilter>
    {
        Task<Response<List<ProgramDto>>> GetDropDown();
    }
}
