using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.FiscalPeriodSetting
{
    public class FiscalPeriodSettingDto
    {
        public int? Id { get; set; }
        public string LastMonth { get; set; }
        public int? LastDay { get; set; }
        public DateOnly? ThresholdDate { get; set; }
        public bool DyanmicReports { get; set; }
    }
}
