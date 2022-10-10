using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        public int? Id { get; set; }
        public int? NoOfIncrements { get; set; }
    }
}
