using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PayrollTransactionMaster : BaseEntity<int>
    {
        [MaxLength(15)]
        public string DocNo { get;private set; }
        public int Month { get;private set; }
        public int Year { get;private set; }
        public int EmployeeId { get;private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get;private set; }
        public int DesignationId { get;private set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get;private set; }
        public int DepartmentId { get;private set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get;private set; }
        public Guid AccountPayableId { get;private set; }
        [ForeignKey("AccountPayableId")]
        public Level4 AccountPayable { get;private set; }
        public int WorkingDays { get;private set; }
        public int PresentDays { get;private set; }
        public int LeaveDays { get;private set; }
        public DateTime TransDate { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossSalary { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalaryBeforeTax { get;private set; }
        public int StatusId { get;private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get;private set; }
        public int? TransactionId { get;private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get;private set; }
        public virtual List<PayrollTransactionLines> PayrollTransactionLines { get;private set; }

        public PayrollTransactionMaster(int month, int year, int employeeId, int designationId, int departmentId, Guid accountPayableId, int workingDays, int presentDays, int leaveDays, DateTime transDate, decimal tax, decimal basicSalary, decimal grossSalary, decimal netSalaryBeforeTax, int statusId, int? transactionId, List<PayrollTransactionLines> payrollTransactionLines)
        {
            Month = month;
            Year = year;
            EmployeeId = employeeId;
            DesignationId = designationId;
            DepartmentId = departmentId;
            AccountPayableId = accountPayableId;
            WorkingDays = workingDays;
            PresentDays = presentDays;
            LeaveDays = leaveDays;
            TransDate = transDate;
            Tax = tax;
            BasicSalary = basicSalary;
            GrossSalary = grossSalary;
            NetSalaryBeforeTax = netSalaryBeforeTax;
            StatusId = statusId;
            TransactionId = transactionId;
            PayrollTransactionLines = payrollTransactionLines;
        }


        protected PayrollTransactionMaster()
        {

        }

        public void setStatus(int statusId)
        {
            StatusId = statusId;
        }
        public void setTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "PAYROLL-" + String.Format("{0:000}", Id);
        }
    }
}
