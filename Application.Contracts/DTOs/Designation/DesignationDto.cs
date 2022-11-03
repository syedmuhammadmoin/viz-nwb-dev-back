using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class DesignationDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id can not be set to zero")]
        public int? Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
