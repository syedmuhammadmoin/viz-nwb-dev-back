using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowStatus : BaseEntity<int>
    {
        [Required]
        public string Status { get; private set; }
        [Required]
        public DocumentStatus State { get; private set; }
        public StatusType Type { get; private set; }

        protected WorkFlowStatus()
        {

        }
    }
}
