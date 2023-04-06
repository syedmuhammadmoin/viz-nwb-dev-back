using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IBatchService : ICrudService<CreateBatchDto, BatchDto, int, TransactionFormFilter>
    {
        Task<Response<List<BatchDto>>> GetDropDown();
    }
}
