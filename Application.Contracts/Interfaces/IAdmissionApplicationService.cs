using Application.Contracts.DTOs;
using Application.Contracts.Filters;

namespace Application.Contracts.Interfaces
{
    public interface IAdmissionApplicationService : ICrudService<CreateAdmissionApplicationDto, AdmissionApplicationDto, int, TransactionFormFilter>
    {
    }
}
