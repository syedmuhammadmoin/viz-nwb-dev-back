using Domain.Base;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CurrencyLine: BaseEntity<int>, IMustHaveTenant
    {
        // Foreign key reference to Currency
        public int CurrencyId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal UnitPerUSD { get; private set; }
        public decimal USDPerUnit { get; private set; }
        public int OrganizationId { get;set ; }
    }
}
