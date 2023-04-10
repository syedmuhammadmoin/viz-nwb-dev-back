using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IApplicantService : ICrudService<CreateApplicantDto, ApplicantDto, int, TransactionFormFilter>
    {
        Task<Response<List<ApplicantDto>>> GetDropDown();
        Task<Response<string>> LoginApplicant(LoginDto entity);
        Task<Response<int>> RegisterApplicant(RegisterApplicantDto entity);
    }
}
