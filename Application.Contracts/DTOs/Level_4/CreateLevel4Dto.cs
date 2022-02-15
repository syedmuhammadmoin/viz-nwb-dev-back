using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateLevel4Dto
    {
        public Guid? Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid Level3_id { get; set; }
    }
}
