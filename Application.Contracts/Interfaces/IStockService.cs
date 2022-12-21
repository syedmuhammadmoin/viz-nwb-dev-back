using Application.Contracts.DTOs;
using Application.Contracts.DTOs.Stock;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IStockService
    {
        Task<PaginationResponse<List<StockDto>>> GetAllAsync(TransactionFormFilter filter);
        Response<StockDto> GetStockByItemAndWarehouse(GetStockByItemAndWarehouseDto stock);
    }
}
