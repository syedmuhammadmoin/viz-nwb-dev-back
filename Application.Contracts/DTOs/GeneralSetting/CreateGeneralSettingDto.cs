using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.GeneralSetting
{
    public class CreateGeneralSettingDto
    {
        public int Id { get; set; }
        public bool? ComTransactions { set; get; }
        public string HeaderColor { set; get; }
        public string ButtonColor { set; get; }
        public bool? IsKilogram { set; get; }
        public bool? IsPound { set; get; }
        public bool? IsCubicMeters { set; get; }
        public bool? IsCubicFeet { set; get; }
    }
}
