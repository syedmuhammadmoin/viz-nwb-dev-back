﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UpdateEmployeeTransactionDto
    {
        public int Id { get; set; }
        [MaxLength(15)]
        public string DocNo { get; set; }   
        public int CampusId { get; set; }                            
        public DateTime TransDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal GrossSalary { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal NetSalary { get; set; }
		public int EmployeeId { get; set; }                   
        [MaxLength(40)]
        public string EmployeeType { get; set; }   
        [MaxLength(30)]      
        public string Religion { get; set; }         
        public int DesignationId { get; set; }      
        public int DepartmentId { get; set; }              
        public decimal NetIncrement { get; set; }      
        public virtual List<PayrollTransactionLines> payrollTransactionLines { get; set; } = new List<PayrollTransactionLines>();

    }
}
