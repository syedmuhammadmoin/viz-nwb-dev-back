using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Location : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(50)]
        public string Dimensions { get; private set; }
        [MaxLength(50)]
        public string Supervisor { get; private set; }

        public int WarehouseId { get; private set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; private set; }

        public Location(Location location)
        {
            Name = location.Name;
            Dimensions = location.Dimensions;
            Supervisor = location.Supervisor;
            WarehouseId = location.WarehouseId;
        }

        protected Location()
        {

        }
    }
}
