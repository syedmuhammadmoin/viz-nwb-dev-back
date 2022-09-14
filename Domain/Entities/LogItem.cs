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
        
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string TraceId { get; set; }

    }

}
