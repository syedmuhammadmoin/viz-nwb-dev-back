﻿using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string EmployeeType { get; set;}
        public string BankName { get; set;}
        public string BranchName { get; set;}
        public string AccountTitle { get; set;}
        public string AccountNumber { get; set;}
        public string EmployeeCode { get; set;}
        public string Domicile { get;  set; }
        public string Contact { get;  set; }
        public string Religion { get;  set; }
        public string Nationality { get;  set; }
        public string Maritalstatus { get;  set; }
        public string Gender { get;  set; }
        public int BasicPayItemId { get; set; }
        public int? IncrementItemId { get; set; }
        public string BPS { get; set; }
        public string BPSAccountId { get; set; }
        public string PlaceofBirth { get;  set; }
        public int BusinessPartnerId { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int DepartmentId { get;  set; }
        public string DepartmentName { get;  set; }
        public int CampusId { get;  set; }
        public string CampusName { get;  set; }
        public string Address { get;  set; }
        public DateTime DateofJoining { get;  set; }
        public DateTime DateofRetirment { get;  set; }
        public DateTime DateofBirth { get;  set; }
        public int EarnedLeaves { get;  set; }
        public int CasualLeaves { get;  set; }
        public bool isActive { get;  set; }
        public string Role { get;  set; }
        public string Faculty { get;  set; }
        public string DutyShift { get;  set; }
        public decimal BasicPay { get; set; }
        public decimal IncrementAmount { get; set; }
        public string IncrementName { get; set; }
        public int? NoOfIncrements { get; set; }
        public decimal TotalIncrement { get; set; }
        public decimal TotalBasicPay { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal GrossPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal NetPay { get; set; }
        public virtual List<PayrollItemDto> PayrollItems { get; set; }
        public string Email { get; set; }
        public string? AccountPayableId { get; set; }
        public string AccountPayableName { get; set; }
    }
}
