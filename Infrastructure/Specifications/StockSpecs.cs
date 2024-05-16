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
        public StockSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Item.ProductName.Contains(filter.Name != null ? filter.Name : "")
            && c.Warehouse.Name.Contains(filter.Warehouse != null ? filter.Warehouse : "")
            && (c.AvailableQuantity == Convert.ToInt32(filter.AvailableQuantity))
			&& (c.ReservedQuantity == Convert.ToInt32(filter.ReservedQuantity))
            && c.Item.Category.Name.Contains(filter.Category != null ? filter.Category : "")
			 && c.CreatedDate.Value.Month == (filter.Month != null ? Convert.ToInt32(filter.Month) : c.CreatedDate.Value.Month)
			&& c.CreatedDate.Value.Year == (filter.Year != null ? Convert.ToInt32(filter.Year) : c.CreatedDate.Value.Year)
			&& c.Item.UnitOfMeasurement.Name.Contains(filter.UnitOfMeasurement != null ? filter.UnitOfMeasurement : "")            
			)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Warehouse);
                AddInclude("Item.Category");
                AddInclude("Item.UnitOfMeasurement");
                
            }
        }

        public StockSpecs(List<int> itemIds) 
            : base (i => (itemIds.Count() > 0 ? itemIds.Contains(i.ItemId) : true))
        {
        }

        public StockSpecs(int itemId, int warehouseId) : base(x => x.ItemId == itemId && x.WarehouseId == warehouseId)
        {
        }
    }
}
