using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IFeeItemService : ICrudService<CreateFeeItemDto, FeeItemDto, int, TransactionFormFilter>
    {
        Task<Response<List<FeeItemDto>>> GetDropDown();
    }
}
