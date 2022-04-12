using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string FatherName { get; set; }
        [Required]
        [MaxLength(20)]
        public string CNIC { get; set; }
        [Required]
        [MaxLength(100)]
        public string Domicile { get; set; }
        [Required]
        [MaxLength(20)]
        public string Contact { get; set; }
        [Required]
        [MaxLength(20)]
        public string Religion { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nationality { get; set; }
        [Required]
        [MaxLength(50)]
        public string Maritalstatus { get; set; }
        [Required]
        [MaxLength(20)]
        public string Gender { get; set; }
        [MaxLength(50)]
        public string PlaceofBirth { get; set; }
        [Required]
        public int DesignationId { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Address { get; set; }
        [Required]
        public DateTime DateofJoining { get; set; }
        public DateTime? DateofRetirment { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int? EarnedLeaves { get; set; }
        public int? CasualLeaves { get; set; }
        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
        [Required]
        [MaxLength(50)]
        public string Role { get; set; }
        [Required]
        [MaxLength(80)]
        public string Faculty { get; set; }
        [Required]
        [MaxLength(80)]
        public string DutyShift { get; set; }

    }
}
