using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class QuotationComparativeLines : BaseEntity<int>
    {
        public int QoutationIds { get; private set; }
        [ForeignKey("QoutationIds")]
        public QuotationMaster QuotationMaster { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public QuotationComparativeMaster quotationComparative { get; private set; }
        protected QuotationComparativeLines()
        {
        }
    }
}
