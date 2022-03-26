using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class WorkFlowTransitionDto
    {
        public int Id { get; set; }
        public int CurrentStatusId { get; set; }
        public string CurrentStatus { get; set; }
        public ActionButton Action { get; set; }
        public int NextStatusId { get; set; }
        public string NextStatus { get; set; }
        public string AllowedRoleId { get; set; }
        public string AllowedRole { get; set; }
        public int MasterId { get; set; }
    }
}
