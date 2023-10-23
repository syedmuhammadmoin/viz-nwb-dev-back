using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IShiftService : ICrudService<ShiftDto, ShiftDto, int, TransactionFormFilter>
    {
        Task<Response<List<ShiftDto>>> GetDropDown();
    }
}
