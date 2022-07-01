using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class StockSpecs : BaseSpecification<Stock>
    {
        public StockSpecs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i => i.Warehouse);
            AddInclude("Item.Category");
            AddInclude("Item.UnitOfMeasurement");
        }
        public StockSpecs(int itemId, int warehouseId) : base( x => x.ItemId == itemId && x.WarehouseId == warehouseId)
        {
        }
    }
}
