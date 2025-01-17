﻿using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
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
        [StringLength(13, MinimumLength = 13, ErrorMessage ="Please enter valid CNIC Number of 13 Digits without Dashes")]
        public string CNIC { get; set; }
        [Required]
        [MaxLength(40)]
        public string EmployeeType { get; set; }
        [Required]
        [MaxLength(50)]
        public string BankName { get; set; }
        [Required]
        [MaxLength(50)]
        public string BranchName { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountTitle { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public string EmployeeCode { get; set; }
        [MaxLength(100)]
        public string Domicile { get; set; }
        [MaxLength(20)]
        public string Contact { get; set; }
        [MaxLength(20)]
        public string Religion { get; set; }
        [MaxLength(50)]
        public string Nationality { get; set; }
        [MaxLength(50)]
        public string Maritalstatus { get; set; }
        [MaxLength(20)]
        public string Gender { get; set; }
        [MaxLength(50)]
        public string PlaceofBirth { get; set; }
        [Required]
        public int? DesignationId { get; set; }
        [Required]
        public int? DepartmentId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Address { get; set; }
        [Required]
        public DateTime? DateofJoining { get; set; }
        public DateTime? DateofRetirment { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int? EarnedLeaves { get; set; }
        public int? CasualLeaves { get; set; }
        [Required]
        public bool? isActive { get; set; }
        [MaxLength(80)]
        public string Faculty { get; set; }
        [MaxLength(80)]
        public string DutyShift { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
