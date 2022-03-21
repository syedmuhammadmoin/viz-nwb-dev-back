using Domain.Constants;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class WorkFlowStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        public StatusType Type { get; set; }
    }
}
