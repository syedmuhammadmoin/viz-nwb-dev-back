using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GeneralSettingEntity : BaseEntity<int>
    {
        public bool? ComTransactions { private set; get; }
        public string HeaderColor { private set; get; }
        public string ButtonColor { private set; get; }
        public bool? IsKilogram { private set; get; }
        public bool? IsPound { private set; get; }
        public bool? IsCubicMeters { private set; get; }
        public bool? IsCubicFeet { private set; get; }
        public int? OrganizationId { private set; get; }
        [ForeignKey("OrganizationId")]
        public Organization? Organization { private set; get; }

    }
}
