using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IDistrictService : ICrudService<CreateDistrictDto, DistrictDto, int, TransactionFormFilter>
    {
        Task<Response<List<DistrictDto>>> GetDropDown();
        Task<Response<List<DistrictDto>>> GetByCity(int cityId);
    }
}
