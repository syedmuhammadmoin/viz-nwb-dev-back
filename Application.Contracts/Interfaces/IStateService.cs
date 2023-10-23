using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IStateService : ICrudService<CreateStateDto, StateDto, int, TransactionFormFilter>
    {
        Task<Response<List<StateDto>>> GetDropDown();
        Task<Response<List<StateDto>>> GetByCountry(int countryId);
    }
}
