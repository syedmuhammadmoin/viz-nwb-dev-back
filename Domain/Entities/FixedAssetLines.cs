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
    public class FixedAssetLines : BaseEntity<int>
    {
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public FixedAsset FixedAsset { get; private set; }
        public int ActiveDays { get; private set; }
        public DateTime ActiveDate { get; private set; }
        public DateTime? InactiveDate { get; private set; }
        protected FixedAssetLines()
        {
        }

        public void SetInactiveDate(DateTime? inactiveDate)
        {
            InactiveDate = inactiveDate;
        }

        public void SetActiveDays(int activeDays)
        {
             ActiveDays = activeDays;
        }
    }
}
