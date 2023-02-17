using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IDepreciationModelService : ICrudService<CreateDepreciationModelDto, DepreciationModelDto, int, TransactionFormFilter>
    {
        Task<Response<List<DepreciationModelDto>>> GetDropDown();
    }
}
