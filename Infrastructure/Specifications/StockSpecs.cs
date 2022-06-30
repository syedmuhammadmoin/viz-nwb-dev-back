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
        public StockSpecs(int itemId, int warehouseId) : base( x => x.ItemId == itemId && x.WarehouseId == warehouseId)
        {
        }
    }
}
