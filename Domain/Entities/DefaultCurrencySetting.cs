using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DefaultCurrencySetting : BaseEntity<int>
    {
        public int? CurrencyId { get; private set; }
        [ForeignKey("CurrencyId")]
        public Currency? Currency { get; private set; }
        public int? OrganizationId { get; private set; }
        [ForeignKey("OrganizationId")]
        public Organization? Organization { get; private set; }
    }
}
