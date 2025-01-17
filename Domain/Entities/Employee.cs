﻿using Domain.Base;
using Domain.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(100)]
        public string FatherName { get; private set; }
        [MaxLength(20)]
        public string CNIC { get; private set; }
        [MaxLength(40)]
        public string EmployeeType { get; private set; } 
        [MaxLength(50)]
        public string BankName { get; private set; }
        [MaxLength(50)]
        public string BranchName { get; private set; }
        [MaxLength(50)]
        public string AccountTitle { get; private set; }
        [MaxLength(50)]
        public string AccountNumber { get; private set; }
        [MaxLength(30)]
        public string EmployeeCode { get; private set; }
        [MaxLength(100)]
        public string Domicile { get; private set; }
        [MaxLength(20)]
        public string Contact { get; private set; }
        [MaxLength(20)]
        public string Religion { get; private set; }
        [MaxLength(50)]
        public string Nationality { get; private set; }
        [MaxLength(50)]
        public string Maritalstatus { get; private set; }
        [MaxLength(20)]
        public string Gender { get; private set; }
        [MaxLength(50)]
        public string PlaceofBirth { get; private set; }
        public int BusinessPartnerId { get; private set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }
        public int DesignationId { get; private set; }
        [ForeignKey("DesignationId")] 
        public Designation Designation { get; private set; }
        public int DepartmentId { get; private set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; private set; }
        [MaxLength(300)]
        public string Address { get; private set; }
        public DateTime DateofJoining { get; private set; }
        public DateTime DateofRetirment { get; private set; }
        public DateTime DateofBirth { get; private set; }
        public int EarnedLeaves { get; private set; }
        public int CasualLeaves { get; private set; }
        [MaxLength(80)]
        public string Faculty { get; private set; }
        [MaxLength(80)]
        public string DutyShift { get; private set; }
        public int? NoOfIncrements { get; private set; }
        [MaxLength(100)]
        public string Email { get; private set; }
        public bool isActive { get; private set; }

        public void SetBusinessPartnerId(int businessPartnerId) 
        {
            BusinessPartnerId = businessPartnerId;
        }
        public void SetEmployeeEmail(string email)
        {
            Email = email;
        }
        
        public void UpdateEmployee(int? noOfIncrements)
        {
            NoOfIncrements = noOfIncrements;
        }

        protected Employee()
        {

        }
    }
}
