﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public enum Roles
    {
        SuperAdmin,
        Admin
    }
    public enum BusinessPartnerType 
    {
        Customer,
        Vendor,
        Employee,
        Supplier, 
        Consultant, 
        Contractor
    }
    public enum ProductType
    {
        Consumable,
        Service,
        FixedAsset
    }
    public enum DocumentStatus
    {
        Draft,
        Rejected,
        Unpaid,
        Partial,
        Paid,
        Submitted,
        Reviewed,
        Cancelled,
        Unreconciled,
        Reconciled
    }
    public enum PaymentType
    {
        Inflow,
        Outflow
    }
    public enum PaymentRegisterType
    {
        CashAccount,
        BankAccount
    }
    public enum DocType
    {
        Payment,
        CreditNote,
        DebitNote,
        Invoice,
        Bill,
        JournalEntry,
        BankAccount,
        CashAccount,
        PurchaseOrder,
        SalesOrder,
        GRN,
        GDN,
        InventoryAdjustment,
        Quotation,
        Requisition,
        Receipt,
        PayrollTransaction,
        PayrollPayment,
        Issuance,
        GoodsReturnNote,
        IssuanceReturn
    }
    public enum ActionButton
    {
        Approve,
        Reject
    }
    public enum StatusType
    {
        Custom,
        PreDefined,
        PreDefinedInList,
    }
    public enum BankAccountType
    {
        Current,
        Saving
    }
    public enum CalculationType
    {
        Percentage,
        FixedAmount
    }
    public enum PayrollType
    {
        BasicPay,
        Increment,
        Deduction,
        Allowance,
        AssignmentAllowance,
        TaxDeduction
    }
    public enum TaxType
    {
        SalesTaxAsset,
        SalesTaxLiability,
        IncomeTaxAsset,
        IncomeTaxLiability,
        SRBTaxAsset,
        SRBTaxLiability
    }
}
