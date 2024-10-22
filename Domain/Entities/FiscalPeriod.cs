using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FiscalPeriod : BaseEntity<int>
    {
        public string Name { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public int OrganizationId { get; private set; }
        [ForeignKey("OrganizationId")]
        public Organization Organization { get; private set; }
    }
}
