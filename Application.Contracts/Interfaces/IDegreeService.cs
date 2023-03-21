using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IDegreeService : ICrudService<DegreeDto, DegreeDto, int, TransactionFormFilter>
    {
        Task<Response<List<DegreeDto>>> GetDropDown();
    }
}
