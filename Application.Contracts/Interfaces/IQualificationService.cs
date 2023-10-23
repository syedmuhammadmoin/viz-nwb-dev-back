using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IQualificationService : ICrudService<QualificationDto, QualificationDto, int, TransactionFormFilter>
    {
        Task<Response<List<QualificationDto>>> GetDropDown();
    }
}
