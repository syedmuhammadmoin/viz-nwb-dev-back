using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTO
{
    public class ContactUsDto
    {
        [Required]
        public string Name { get; set; }
        [Required] 
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string NoOfEmployees { get; set; }
        public string BusinessDomain { get; set; }
        public string Subject { get; set; }
        public string Query { get; set; }
    }
}
