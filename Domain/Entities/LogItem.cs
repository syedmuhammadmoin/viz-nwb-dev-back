using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LogItem : BaseEntity<Guid>
    { 
        
        [MaxLength(1000)]
        public string Message { get; set; }
        public int Status { get; set; }
        [MaxLength(5000)]
        public string Detail { get; set; }
        [MaxLength(500)]
        public string TraceId { get; set; }

    }

}
