using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public enum Roles
    {
        SuperAdmin = 0,
        Admin = 1
    }
    public enum BusinessPartnerType 
    {
        Customer = 0,
        Vendor = 1,
        Employee = 2,
        Supplier = 3, 
        Consultant = 4, 
        Contractor = 5
    }
    public enum ProductType
    {
        Consumable = 0,
        Service = 1,
        FixedAsset = 2
    }
    public enum DocumentStatus
    {
        Draft = 0,
        Rejected = 1,
        Unpaid = 2,
        Partial = 3,
        Paid = 4,
        Submitted = 5,
        Reviewed = 6,
        Cancelled = 7,
        Unreconciled = 8,
        Reconciled = 9
    }
    public enum PaymentType
    {
        Inflow = 0,
        Outflow = 1 
    }
    public enum PaymentRegisterType
    {
        CashAccount = 0,
        BankAccount = 1
    }
    public enum DocType
    {
        Payment = 0,
        CreditNote = 1,
        DebitNote = 2,
        Invoice = 3,
        Bill = 4,
        JournalEntry = 5,
        BankAccount = 6 ,
        CashAccount = 7,
        PurchaseOrder = 8,
        SalesOrder = 9,
        GRN = 10,
        GDN = 11,
        InventoryAdjustment = 12,
        Quotation = 13,
        Requisition = 14,
        Receipt = 15,
        PayrollTransaction = 16,
        PayrollPayment = 17,
        Issuance = 18,
        GoodsReturnNote = 19,
        IssuanceReturn = 20,
        Request = 21,
        BidEvaluation = 22,
        CallForQuotaion = 23,
        QuotationComparative = 24,
        FixedAsset = 25,
        CWIP = 26,
        Disposal = 27,
        BudgetReappropriation = 28,
        DepreciationAdjustment = 29
    }
    public enum ActionButton
    {
        Approve = 0,
        Reject = 1
    }
    public enum StatusType
    {
        Custom = 0,
        PreDefined = 1,
        PreDefinedInList = 2,
    }
    public enum BankAccountType
    {
        Current = 0,
        Saving = 1
    }
    public enum CalculationType
    {
        Percentage = 0,
        FixedAmount = 1 
    }
    public enum PayrollType
    {
        BasicPay = 0,
        Increment = 1,
        Deduction = 2,
        Allowance = 3,
        AssignmentAllowance = 4,
        TaxDeduction = 5
    }
    public enum TaxType
    {
        SalesTaxAsset = 0,
        SalesTaxLiability = 1,
        IncomeTaxAsset = 2,
        IncomeTaxLiability = 3,
        SRBTaxAsset = 4,
        SRBTaxLiability = 5  
    }
    public enum AccountType
    {
        SystemDefined = 0,
        UserDefined = 1
    }
    public enum DepreciationMethod 
    {
        StraightLine = 0,
        Declining = 1 
    }
}
