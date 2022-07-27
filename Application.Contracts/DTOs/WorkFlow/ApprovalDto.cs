using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class ApprovalDto
    {
        public int DocId { get; set; }
        public ActionButton Action { get; set; }
        public string Remarks { get; set; }
    }
}
