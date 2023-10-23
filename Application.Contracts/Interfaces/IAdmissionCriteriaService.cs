using Application.Contracts.DTOs;
using Application.Contracts.Filters;

namespace Application.Contracts.Interfaces
{
    public interface IAdmissionCriteriaService : ICrudService<CreateAdmissionCriteriaDto, AdmissionCriteriaDto, int, TransactionFormFilter>
    {
    }
}
