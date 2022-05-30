﻿using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class PayrollTransactionDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string BPSName { get; set; }
        public Guid AccountPayableId { get; set; }
        public string AccountPayable { get; set; }
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int LeaveDays { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal GrossPay { get; set; }
        public decimal NetSalary { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DocumentStatus State { get; set; }
        //public IEnumerable<PaidDocList> PaidAmountList { get; set; }
       // public decimal PendingAmount { get; set; }
       // public decimal TotalPaid { get; set; }
        public bool IsAllowedRole { get; set; }
        public int TransactionId { get; set; }
        public decimal BasicSalary { get; set; }
        public string CNIC { get; set; }
        public string Religion { get; set; }
        public DateTime TransDate { get; set; }
        public virtual List<PayrollTransactionLinesDto> PayrollTransactionLines { get; set; }

    }
}
