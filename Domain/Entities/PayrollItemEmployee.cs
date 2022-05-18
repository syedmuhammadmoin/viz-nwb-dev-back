﻿using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PayrollItemEmployee : BaseEntity<int>
    {
        public int EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; private set; }
        public int PayrollItemId { get; private set; }
        [ForeignKey("PayrollItemId")]
        public PayrollItem PayrollItem { get; private set; }
            
        public PayrollItemEmployee(int employeeId, int payrollItemId)
        {
            EmployeeId = employeeId;
            PayrollItemId = payrollItemId;
        }
    }
}