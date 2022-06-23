using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UnitOfMeasurement : BaseEntity<int>
    {
        [MaxLength(50)]
        public string Name { get; private set; }
        protected UnitOfMeasurement()
        {

        }
    }
}
