using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateDeptDto
    {
        public int? Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(20)]
        public string Street { get; set; }
        [MaxLength(20)]
        public string Block { get; set; }
        [MaxLength(20)]
        public string Road { get; set; }
        [Required]
        [MaxLength(100)]
        public string HeadOfDept { get; set; }
        [Required]
        public int OrganizationId { get; set; }
    }
}