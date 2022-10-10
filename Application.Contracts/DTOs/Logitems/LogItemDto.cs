using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class LogItemDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int? Status { get; set; }
        [Required]
        public string Detail { get; set; }
        [Required]
        public string TraceId { get; set; }
    }
}
