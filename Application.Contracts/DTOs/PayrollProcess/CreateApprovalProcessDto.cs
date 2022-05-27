using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateApprovalProcessDto
    {
        public int[] docId { get; set; }
        public ActionButton Action { get; set; }
    }
}
