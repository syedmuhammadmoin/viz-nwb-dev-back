using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IDomicileService : ICrudService<CreateDomicileDto, DomicileDto, int, TransactionFormFilter>
    {
        Task<Response<List<DomicileDto>>> GetDropDown();
        Task<Response<List<DomicileDto>>> GetByDistrict(int districtId);
    }
}
