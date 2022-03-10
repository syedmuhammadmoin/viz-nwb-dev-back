using System;
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
        Employee
    }
    public enum PurchasedOrSold
    { 
        Purchased,
        Sold
    }
    public enum ProductType
    {
        Consumable,
        Service
    }
    public enum DocumentStatus
    {
        Draft,
        Submitted
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
        Requisition
    }
    public enum ReconStatus
    {
        Unreconciled,
        Partial,
        Reconciled
    }
}
