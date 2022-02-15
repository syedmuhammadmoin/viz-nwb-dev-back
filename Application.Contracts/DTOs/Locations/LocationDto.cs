using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dimensions { get; set; }
        public string Supervisor { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
}
