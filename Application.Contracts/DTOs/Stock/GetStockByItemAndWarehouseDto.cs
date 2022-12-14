using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Stock
{
    public class GetStockByItemAndWarehouseDto
    {

        public int ItemId { get; set; }
        public int WarehouseId { get; set; }
    }
}
