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
        public Guid BPSAccountId { get; private set; }
        public string BPSName { get; private set; }
        public int CampusId { get;private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get;private set; }
        public int DesignationId { get;private set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get;private set; }
        public int DepartmentId { get;private set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get;private set; }
        public Guid? AccountPayableId { get;private set; }
        [ForeignKey("AccountPayableId")]
        public Level4 AccountPayable { get;private set; }
        public int WorkingDays { get;private set; }
        public int PresentDays { get;private set; }
        public int LeaveDays { get;private set; }
        public DateTime TransDate { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossSalary { get;private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get;private set; }
        public int StatusId { get;private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get;private set; }
        public int? TransactionId { get;private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get;private set; }
        public int? LedgerId { get; private set; }
        public virtual List<PayrollTransactionLines> PayrollTransactionLines { get;private set; }

        public PayrollTransactionMaster(int month, int year, int employeeId, Guid bpsAccountId, string bpsName, int designationId, int departmentId, int campusId, int workingDays, int presentDays, int leaveDays, DateTime transDate, decimal basicSalary, decimal grossSalary, decimal netSalary, int statusId, List<PayrollTransactionLines> payrollTransactionLines)
        {
            Month = month;
            Year = year;
            EmployeeId = employeeId;
            BPSAccountId = bpsAccountId;
            BPSName = bpsName;
            DesignationId = designationId;
            DepartmentId = departmentId;
            CampusId = campusId;
            WorkingDays = workingDays;
            PresentDays = presentDays;
            LeaveDays = leaveDays;
            TransDate = transDate;
            BasicSalary = basicSalary;
            GrossSalary = grossSalary;
            NetSalary = netSalary;
            StatusId = statusId;
            PayrollTransactionLines = payrollTransactionLines;
        }


        public void updatePayrollTransaction(Guid bpsAccountId, string bpsName, int designationId, int departmentId, int workingDays, int presentDays, int leaveDays, DateTime transDate, decimal basicSalary, decimal grossSalary, decimal netSalary, List<PayrollTransactionLines> payrollTransactionLines)
        {
            BPSAccountId = bpsAccountId;
            BPSName = bpsName;
            DesignationId = designationId;
            DepartmentId = departmentId;
            WorkingDays = workingDays;
            PresentDays = presentDays;
            LeaveDays = leaveDays;
            TransDate = transDate;
            BasicSalary = basicSalary;
            GrossSalary = grossSalary;
            NetSalary = netSalary;
            PayrollTransactionLines = payrollTransactionLines;
        }
        public void updateAccountPayableId(Guid accountPayableId,int statusId)
        {
            AccountPayableId = accountPayableId;
            StatusId = statusId;
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
        public void setLedgerId(int ledgerId)
        {
            LedgerId = ledgerId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "PAYROLL-" + String.Format("{0:000}", Id);
        }
    }
}
