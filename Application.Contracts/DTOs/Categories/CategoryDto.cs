using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid InventoryAccountId { get; set; }
        public Guid RevenueAccountId { get; set; }
        public Guid CostAccountId { get; set; }
        public string InventoryAccount { get; set; }
        public string RevenueAccount { get; set; }
        public string CostAccount { get; set; }
    }
}
