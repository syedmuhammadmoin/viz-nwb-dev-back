using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface ICityService : ICrudService<CreateCityDto, CityDto, int, TransactionFormFilter>
    {
        Task<Response<List<CityDto>>> GetDropDown();
        Task<Response<List<CityDto>>> GetByState(int stateId);
    }
}
