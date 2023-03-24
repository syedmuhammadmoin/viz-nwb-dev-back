using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IProgramService : ICrudService<CreateProgramDto, ProgramDto, int, TransactionFormFilter>
    {
        Task<Response<List<ProgramDto>>> GetDropDown();
        Task<Response<int>> AddFees(List<AddSemesterFeesDto> entity);
        Task<Response<List<ProgramSemesterDto>>> GetProgramSemesters(int programId);
    }
}
