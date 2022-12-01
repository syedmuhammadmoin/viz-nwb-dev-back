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
        public int CampusId { get;private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get;private set; }
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

        /// <summary>
        /// Employee Fields
        /// </summary>
        public int EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; private set; }

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

        /// <summary>
        /// Payroll Items for Basic Pay
        /// </summary>
        public int BasicPayItemId { get; private set; }
        [ForeignKey("BasicPayItemId")]
        public PayrollItem BasicPayItem { get; private set; }

        public Guid BPSAccountId { get; private set; }
        [ForeignKey("BPSAccountId")]
        public Level4 BPSAccount { get; private set; }
        
        [MaxLength(500)]
        public string BPSName { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BPSAmount { get; private set; }
        /// <summary>
        /// Payroll Items for Increment
        /// </summary>
        public int? IncrementItemId { get; private set; }
        [ForeignKey("IncrementItemId")]
        public PayrollItem IncrementItem { get; private set; }
        [MaxLength(500)]
        public string IncrementName { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal IncrementAmount { get; private set; }

        // Payroll Lines
        public virtual List<PayrollTransactionLines> PayrollTransactionLines { get;private set; }

        //Construtors
        //public PayrollTransactionMaster(int month, int year, int employeeId, Guid bpsAccountId, string bpsName, int designationId, int departmentId, int campusId, int workingDays, int presentDays, int leaveDays, DateTime transDate, decimal basicSalary, decimal grossSalary, decimal netIncrement, decimal netSalary, int statusId, string employeeType, List<PayrollTransactionLines> payrollTransactionLines)
        //{
        //    Month = month;
        //    Year = year;
        //    EmployeeId = employeeId;
        //    BPSAccountId = bpsAccountId;
        //    BPSName = bpsName;
        //    DesignationId = designationId;
        //    DepartmentId = departmentId;
        //    CampusId = campusId;
        //    WorkingDays = workingDays;
        //    PresentDays = presentDays;
        //    LeaveDays = leaveDays;
        //    TransDate = transDate;
        //    BasicSalary = basicSalary;
        //    GrossSalary = grossSalary;
        //    NetSalary = netSalary;
        //    NetIncrement = netIncrement;
        //    StatusId = statusId;
        //    EmployeeType = employeeType;
        //    PayrollTransactionLines = payrollTransactionLines;
        //}

        public PayrollTransactionMaster(int month, int year, Guid bPSAccountId, string bPSName,
            int campusId, int workingDays, int presentDays, int leaveDays,
            DateTime transDate, decimal basicSalary, decimal grossSalary, decimal netSalary,
            int statusId, int employeeId, string name, string fatherName, string cNIC, string employeeType,
            string bankName, string branchName, string accountTitle, string accountNumber, string employeeCode,
            string domicile, string contact, string religion, string nationality, string maritalstatus, string gender,
            string placeofBirth, int designationId, int departmentId, string address, DateTime dateofJoining,
            DateTime dateofRetirment, DateTime dateofBirth, string faculty,
            string dutyShift, int? noOfIncrements, string email, int basicPayItemId, decimal bpsAmount, 
            int? incrementItemId, string incrementName, decimal incrementAmount,
            List<PayrollTransactionLines> payrollTransactionLines)
        {
            Month = month;
            Year = year;
            BPSAccountId = bPSAccountId;
            BPSName = bPSName;
            CampusId = campusId;
            WorkingDays = workingDays;
            PresentDays = presentDays;
            LeaveDays = leaveDays;
            TransDate = transDate;
            BasicSalary = basicSalary;
            GrossSalary = grossSalary;
            NetSalary = netSalary;
            StatusId = statusId;
            EmployeeId = employeeId;
            Name = name;
            FatherName = fatherName;
            CNIC = cNIC;
            EmployeeType = employeeType;
            BankName = bankName;
            BranchName = branchName;
            AccountTitle = accountTitle;
            AccountNumber = accountNumber;
            EmployeeCode = employeeCode;
            Domicile = domicile;
            Contact = contact;
            Religion = religion;
            Nationality = nationality;
            Maritalstatus = maritalstatus;
            Gender = gender;
            PlaceofBirth = placeofBirth;
            DesignationId = designationId;
            DepartmentId = departmentId;
            Address = address;
            DateofJoining = dateofJoining;
            DateofRetirment = dateofRetirment;
            DateofBirth = dateofBirth;
            Faculty = faculty;
            DutyShift = dutyShift;
            NoOfIncrements = noOfIncrements;
            Email = email;
            BasicPayItemId = basicPayItemId;
            BPSAmount = bpsAmount;
            IncrementItemId = incrementItemId;
            IncrementName = incrementName;
            IncrementAmount = incrementAmount;
            PayrollTransactionLines = payrollTransactionLines;
        }

        //public void updatePayrollTransaction(Guid bpsAccountId, string bpsName, int designationId, int departmentId, int workingDays, int presentDays, int leaveDays, DateTime transDate, decimal basicSalary, decimal grossSalary, decimal netSalary, string employeeType, List<PayrollTransactionLines> payrollTransactionLines)
        //{
        //    BPSAccountId = bpsAccountId;
        //    BPSName = bpsName;
        //    DesignationId = designationId;
        //    DepartmentId = departmentId;
        //    WorkingDays = workingDays;
        //    PresentDays = presentDays;
        //    LeaveDays = leaveDays;
        //    TransDate = transDate;
        //    BasicSalary = basicSalary;
        //    GrossSalary = grossSalary;
        //    NetSalary = netSalary;
        //    EmployeeType = employeeType;
        //    PayrollTransactionLines = payrollTransactionLines;
        //}

        public void updatePayrollTransaction(Guid bPSAccountId, string bPSName,
            int campusId, int workingDays, int presentDays, int leaveDays,
            DateTime transDate, decimal basicSalary, decimal grossSalary, decimal netSalary, int statusId,
            string name, string fatherName, string employeeType,
            string bankName, string branchName, string accountTitle, string accountNumber, string employeeCode,
            string domicile, string contact, string religion, string nationality, string maritalstatus, string gender,
            string placeofBirth, int designationId, int departmentId, string address, DateTime dateofJoining,
            DateTime dateofRetirment, DateTime dateofBirth, string faculty,
            string dutyShift, int? noOfIncrements, string email, int basicPayItemId, decimal bpsAmount,
            int? incrementItemId, string incrementName, decimal incrementAmount,
            List<PayrollTransactionLines> payrollTransactionLines)
        {
            BPSAccountId = bPSAccountId;
            BPSName = bPSName;
            CampusId = campusId;
            WorkingDays = workingDays;
            PresentDays = presentDays;
            LeaveDays = leaveDays;
            TransDate = transDate;
            BasicSalary = basicSalary;
            GrossSalary = grossSalary;
            NetSalary = netSalary;
            StatusId = statusId;
            Name = name;
            FatherName = fatherName;
            EmployeeType = employeeType;
            BankName = bankName;
            BranchName = branchName;
            AccountTitle = accountTitle;
            AccountNumber = accountNumber;
            EmployeeCode = employeeCode;
            Domicile = domicile;
            Contact = contact;
            Religion = religion;
            Nationality = nationality;
            Maritalstatus = maritalstatus;
            Gender = gender;
            PlaceofBirth = placeofBirth;
            DesignationId = designationId;
            DepartmentId = departmentId;
            Address = address;
            DateofJoining = dateofJoining;
            DateofRetirment = dateofRetirment;
            DateofBirth = dateofBirth;
            Faculty = faculty;
            DutyShift = dutyShift;
            NoOfIncrements = noOfIncrements;
            Email = email;
            BasicPayItemId = basicPayItemId;
            BPSAmount = bpsAmount;
            IncrementItemId = incrementItemId;
            IncrementName = incrementName;
            IncrementAmount = incrementAmount;
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
