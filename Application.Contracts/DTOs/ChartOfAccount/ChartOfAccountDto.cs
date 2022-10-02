using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class ChartOfAccountDto
    {
        [DisplayName("Nature")]
        public string Nature { get; set; }
        [DisplayName("SummaryHead")]
        public string SummaryHead { get; set; }
        [DisplayName("Head")]
        public string Head { get; set; }
        [DisplayName("TransactionalAccount")]
        public string TransactionalAccount { get; set; }
    }
}
