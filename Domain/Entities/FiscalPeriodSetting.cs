using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FiscalPeriodSetting : BaseEntity<int>
    {
        public string? LastMonth { get; private set; }
        public int? LastDay { get; private set; }
        public DateTime? ThresholdDate { get; private set; }
        public bool DyanmicReports { get; private set; }
    }
}
