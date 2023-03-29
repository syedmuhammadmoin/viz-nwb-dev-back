using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface ICountryService : ICrudService<CountryDto, CountryDto, int, TransactionFormFilter>
    {
        Task<Response<List<CountryDto>>> GetDropDown();
    }
}
