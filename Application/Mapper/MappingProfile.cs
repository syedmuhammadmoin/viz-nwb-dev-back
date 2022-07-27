using Application.Contracts.DTOs;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Orgnization Mapping
            CreateMap<Organization, OrganizationDto>();
            CreateMap<CreateOrganizationDto, Organization>();

            // Campus Mapping
            CreateMap<Campus, CampusDto>();
            CreateMap<CampusDto, Campus>()
                .ForMember(core => core.OrganizationId, dto => dto.MapFrom(a => 1));

            // Department Mapping
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();


            // Designation Mapping
            CreateMap<Designation, DesignationDto>();
            CreateMap<DesignationDto, Designation>();

            // Employee Mapping
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dto => dto.DepartmentName, core => core.MapFrom(a => a.Department.Name))
                .ForMember(dto => dto.DesignationName, core => core.MapFrom(a => a.Designation.Name));

            CreateMap<CreateEmployeeDto, Employee>();

            // EmployeeDropDown for payrollPayment
            CreateMap<Employee, EmployeeDropDownPaymentDto>()
                .ForMember(dto => dto.Id, core => core.MapFrom(a => a.BusinessPartnerId))
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Name));

            // Warehouse Mapping
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name));
            CreateMap<CreateWarehouseDto, Warehouse>();

            // Level4 Mapping
            CreateMap<Level4, Level4Dto>()
                .ForMember(dto => dto.Level3Name, core => core.MapFrom(a => a.Level3.Name));
            CreateMap<CreateLevel4Dto, Level4>();

            // Level3 Mapping
            CreateMap<Level3, Level3DropDownDto>();

            // Level1 Mapping
            CreateMap<Level1, Level1Dto>()
                .ForMember(dto => dto.children, core => core.MapFrom(a => a.Level2));
            CreateMap<Level2, Level2Dto>()
                .ForMember(dto => dto.children, core => core.MapFrom(a => a.Level3));
            CreateMap<Level3, Level3Dto>()
                .ForMember(dto => dto.children, core => core.MapFrom(a => a.Level4));

            // Category Mapping
            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.InventoryAccount, core => core.MapFrom(a => a.InventoryAccount.Name))
                .ForMember(dto => dto.RevenueAccount, core => core.MapFrom(a => a.RevenueAccount.Name))
                .ForMember(dto => dto.CostAccount, core => core.MapFrom(a => a.CostAccount.Name));
            CreateMap<CreateCategoryDto, Category>();

            // BusinessPartner Mapping
            CreateMap<BusinessPartner, BusinessPartnerDto>()
                .ForMember(dto => dto.AccountReceivable, core => core.MapFrom(a => a.AccountReceivable.Name))
                .ForMember(dto => dto.AccountPayable, core => core.MapFrom(a => a.AccountPayable.Name));
            CreateMap<CreateBusinessPartnerDto, BusinessPartner>();

            // Product Mapping
            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.CategoryName, core => core.MapFrom(a => a.Category.Name))
                .ForMember(dto => dto.UnitOfMeasurementName, core => core.MapFrom(a => a.UnitOfMeasurement.Name))
                .ForMember(dto => dto.CostAccountId, core => core.MapFrom(a => a.Category.CostAccountId))
                .ForMember(dto => dto.RevenueAccountId, core => core.MapFrom(a => a.Category.RevenueAccountId))
                .ForMember(dto => dto.InventoryAccountId, core => core.MapFrom(a => a.Category.InventoryAccountId));
            CreateMap<CreateProductDto, Product>();

            // JournalEntry Mapping
            CreateMap<JournalEntryMaster, JournalEntryDto>()
                 .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<JournalEntryLines, JournalEntryLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.BusinessPartnerName, core => core.MapFrom(a => a.BusinessPartner.Name));

            CreateMap<CreateJournalEntryDto, JournalEntryMaster>()
               .ForMember(core => core.TotalDebit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Debit)))
               .ForMember(core => core.TotalCredit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Credit))); ;

            CreateMap<CreateJournalEntryLinesDto, JournalEntryLines>();

            // Invoice Mapping
            CreateMap<InvoiceMaster, InvoiceDto>()
              .ForMember(dto => dto.CustomerName, core => core.MapFrom(a => a.Customer.Name))
              .ForMember(dto => dto.ReceivableAccountName, core => core.MapFrom(a => a.ReceivableAccount.Name))
              .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State == DocumentStatus.Unpaid ? "Unpaid" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<InvoiceLines, InvoiceLinesDto>()
              .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));

            CreateMap<CreateInvoiceDto, InvoiceMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.InvoiceLines.Sum(e => e.Quantity * e.Price)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.InvoiceLines.Sum(e => e.Quantity * e.Price * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.InvoiceLines.Sum(e => (e.Quantity * e.Price) + (e.Quantity * e.Price * e.Tax / 100))));

            CreateMap<CreateInvoiceLinesDto, InvoiceLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Price) + (a.Quantity * a.Price * a.Tax / 100)));

            // CreditNote Mapping
            CreateMap<CreditNoteMaster, CreditNoteDto>()
              .ForMember(dto => dto.CustomerName, core => core.MapFrom(a => a.Customer.Name))
               .ForMember(dto => dto.ReceivableAccountName, core => core.MapFrom(a => a.ReceivableAccount.Name))
               .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<CreditNoteLines, CreditNoteLinesDto>()
              .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));

            CreateMap<CreateCreditNoteDto, CreditNoteMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.CreditNoteLines.Sum(e => e.Quantity * e.Price)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.CreditNoteLines.Sum(e => e.Quantity * e.Price * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.CreditNoteLines.Sum(e => (e.Quantity * e.Price) + (e.Quantity * e.Price * e.Tax / 100))));

            CreateMap<CreateCreditNoteLinesDto, CreditNoteLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Price) + (a.Quantity * a.Price * a.Tax / 100)));

            // Bill Mapping
            CreateMap<BillMaster, BillDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
              .ForMember(dto => dto.PayableAccountName, core => core.MapFrom(a => a.PayableAccount.Name))
              .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State == DocumentStatus.Unpaid ? "Unpaid" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.GRNDocNo, core => core.MapFrom(a => a.GRN.DocNo));

            CreateMap<BillLines, BillLinesDto>()
              .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              ;

            CreateMap<CreateBillDto, BillMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => (e.Quantity * e.Cost * e.Tax / 100) + (e.AnyOtherTax))))
               .ForMember(core => core.OtherTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => e.AnyOtherTax)))
               .ForMember(core => core.Tax, dto => dto.MapFrom(a => a.BillLines.Sum(e => (e.Quantity * e.Cost * e.Tax / 100))))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.BillLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100) + (e.AnyOtherTax))));

            CreateMap<CreateBillLinesDto, BillLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100) + (a.AnyOtherTax)));


            // DebitNote Mapping
            CreateMap<DebitNoteMaster, DebitNoteDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
              .ForMember(dto => dto.PayableAccountName, core => core.MapFrom(a => a.PayableAccount.Name))
              .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<DebitNoteLines, DebitNoteLinesDto>()
              .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));

            CreateMap<CreateDebitNoteDto, DebitNoteMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateDebitNoteLinesDto, DebitNoteLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));

            //Payment Mapping
            CreateMap<Payment, PaymentDto>()
                .ForMember(dto => dto.BusinessPartnerName, core => core.MapFrom(a => a.BusinessPartner.Name))
                .ForMember(dto => dto.PaymentRegisterName, core => core.MapFrom(a => a.PaymentRegister.Name))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
                .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.BankReconStatus == DocumentStatus.Unreconciled ? "In Payment" :
                    a.BankReconStatus == DocumentStatus.Partial ? "Partial Reconciled" :
                    a.BankReconStatus == DocumentStatus.Reconciled ? "Reconciled" : a.Status.Status));

            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(core => core.NetPayment, dto => dto.MapFrom(a => (a.GrossPayment - a.IncomeTax - a.SalesTax - a.SRBTax)));

            // CashAccount Mapping
            CreateMap<CashAccount, CashAccountDto>()
                .ForMember(dto => dto.ChAccountName, core => core.MapFrom(a => a.ChAccount.Name))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name));
            CreateMap<CreateCashAccountDto, CashAccount>();
            CreateMap<UpdateCashAccountDto, CashAccount>();

            // BankAccount Mapping
            CreateMap<BankAccount, BankAccountDto>()
                .ForMember(dto => dto.ChAccountName, core => core.MapFrom(a => a.ChAccount.Name))
                .ForMember(dto => dto.ClearingAccount, core => core.MapFrom(a => a.ClearingAccount.Name))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name));
            CreateMap<CreateBankAccountDto, BankAccount>();
            CreateMap<UpdateBankAccountDto, BankAccount>();


            // BankStmt Mapping
            CreateMap<BankStmtMaster, BankStmtDto>()
              .ForMember(dto => dto.BankAccountName, core => core.MapFrom(a => a.BankAccount.BankName));

            CreateMap<BankStmtLines, BankStmtLinesDto>();

            CreateMap<CreateBankStmtDto, BankStmtMaster>();
            CreateMap<CreateBankStmtLinesDto, BankStmtLines>()
                .ForMember(core => core.BankReconStatus, dto => dto.MapFrom(a => DocumentStatus.Unreconciled));

            //Bank Reconciliation
            CreateMap<CreateBankReconDto, BankReconciliation>();

            // WorkFlow Mapping
            CreateMap<WorkFlowMaster, WorkFlowDto>();

            CreateMap<WorkFlowTransition, WorkFlowTransitionDto>()
            .ForMember(dto => dto.CurrentStatus, core => core.MapFrom(a => a.CurrentStatus.Status))
            .ForMember(dto => dto.NextStatus, core => core.MapFrom(a => a.NextStatus.Status))
            .ForMember(dto => dto.AllowedRole, core => core.MapFrom(a => a.AllowedRole.Name));

            CreateMap<CreateWorkFlowDto, WorkFlowMaster>();

            CreateMap<CreateWorkFlowTransitionDto, WorkFlowTransition>();

            // WorkFlowStatus Mapping
            CreateMap<WorkFlowStatus, WorkFlowStatusDto>();
            CreateMap<CreateWorkFlowStatusDto, WorkFlowStatus>()
            .ForMember(core => core.Type, dto => dto.MapFrom(a => StatusType.Custom));

            // Budget Mapping
            CreateMap<BudgetMaster, BudgetDto>();
            CreateMap<BudgetLines, BudgetLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreateBudgetDto, BudgetMaster>();
            CreateMap<CreateBudgetLinesDto, BudgetLines>();

            // PurchaseOrder Mapping
            CreateMap<PurchaseOrderMaster, PurchaseOrderDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
               .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status.State == DocumentStatus.Unpaid ? "Open" :
                    a.Status.State == DocumentStatus.Partial ? "Open" :
                    a.Status.State == DocumentStatus.Paid ? "Closed" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<PurchaseOrderLines, PurchaseOrderLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.Warehouse, core => core.MapFrom(a => a.Warehouse.Name))
              .ForMember(dto => dto.Item, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.PendingQuantity, core => core.MapFrom(a => a.Quantity));


            CreateMap<CreatePurchaseOrderDto, PurchaseOrderMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.PurchaseOrderLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.PurchaseOrderLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.PurchaseOrderLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreatePurchaseOrderLinesDto, PurchaseOrderLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));

            // Requisition Mapping
            CreateMap<RequisitionMaster, RequisitionDto>()
              .ForMember(dto => dto.EmployeeName, core => core.MapFrom(a => a.Employee.Name))
               .ForMember(dto => dto.Campus, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status.State == DocumentStatus.Unpaid ? "Open" :
                    a.Status.State == DocumentStatus.Partial ? "Open" :
                    a.Status.State == DocumentStatus.Paid ? "Closed" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<RequisitionLines, RequisitionLinesDto>()
              .ForMember(dto => dto.Warehouse, core => core.MapFrom(a => a.Warehouse.Name))
              .ForMember(dto => dto.Item, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.PendingQuantity, core => core.MapFrom(a => a.Quantity));

            CreateMap<CreateRequisitionDto, RequisitionMaster>();

            CreateMap<CreateRequisitionLinesDto, RequisitionLines>();

            // GRN Mapping
            CreateMap<GRNMaster, GRNDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
              .ForMember(dto => dto.Type, core => core.MapFrom(a => a.Vendor.BusinessPartnerType))
               .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status.State == DocumentStatus.Unpaid ? "Approved" :
                    a.Status.State == DocumentStatus.Partial ? "Approved" :
                    a.Status.State == DocumentStatus.Paid ? "Approved" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.PODocNo, core => core.MapFrom(a => a.PurchaseOrder.DocNo));

            CreateMap<GRNLines, GRNLinesDto>()
              .ForMember(dto => dto.Warehouse, core => core.MapFrom(a => a.Warehouse.Name))
              .ForMember(dto => dto.Item, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.PendingQuantity, core => core.MapFrom(a => a.Quantity));

            CreateMap<CreateGRNDto, GRNMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.GRNLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.GRNLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.GRNLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateGRNLinesDto, GRNLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));
           
            // EstimatedBudget Mapping
            CreateMap<EstimatedBudgetMaster, EstimatedBudgetDto>()
                .ForMember(dto => dto.From, core => core.MapFrom(a => a.PreviousBudget.From))
                .ForMember(dto => dto.To, core => core.MapFrom(a => a.PreviousBudget.To));
            CreateMap<EstimatedBudgetLines, EstimatedBudgetLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreateEstimatedBudgetDto, EstimatedBudgetMaster>();
            CreateMap<CreateEstimatedBudgetLinesDto, EstimatedBudgetLines>()
                .ForMember(core => core.EstimatedValue,
                dto => dto.MapFrom(a =>
                a.CalculationType == CalculationType.Percentage ? ((a.Amount * a.Value / 100) + (a.Amount))
                : (a.Amount + a.Value)));

            // PayrollItem Mapping
            CreateMap<PayrollItem, PayrollItemDto>()
                .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));
            CreateMap<CreatePayrollItemDto, PayrollItem>();
            CreateMap<PayrollItemEmployee, PayrollItemEmployeeDto>();

            // PayrollTransaction Mapping
            CreateMap<PayrollTransactionMaster, PayrollTransactionDto>()
              .ForMember(dto => dto.Employee, core => core.MapFrom(a => a.Employee.Name))
              .ForMember(dto => dto.CNIC, core => core.MapFrom(a => a.Employee.CNIC))
              .ForMember(dto => dto.BusinessPartnerId, core => core.MapFrom(a => a.Employee.BusinessPartnerId))
              .ForMember(dto => dto.AccountPayable, core => core.MapFrom(a => a.AccountPayable.Name))
              .ForMember(dto => dto.Department, core => core.MapFrom(a => a.Department.Name))
              .ForMember(dto => dto.Designation, core => core.MapFrom(a => a.Designation.Name))
              .ForMember(dto => dto.BPSName, core => core.MapFrom(a => a.BPSName))
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State == DocumentStatus.Unpaid ? "Unpaid" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.NetSalary, core => core.MapFrom(a => a.NetSalary));

            CreateMap<PayrollTransactionLines, PayrollTransactionLinesDto>()
              .ForMember(dto => dto.PayrollItem, core => core.MapFrom(a => a.PayrollItem.Name))
              .ForMember(dto => dto.Account, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreatePayrollTransactionDto, PayrollTransactionMaster>();

            // Tax Mapping
            CreateMap<Taxes, TaxDto>()
                .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));
            CreateMap<UpdateTaxDto, Taxes>();

            // UnitOfMeasurement Mapping
            CreateMap<UnitOfMeasurement, UnitOfMeasurementDto>();
            CreateMap<CreateUnitOfMeasurementDto, UnitOfMeasurement>();

            // Issuance Mapping
            CreateMap<IssuanceMaster, IssuanceDto>()
              .ForMember(dto => dto.EmployeeName, core => core.MapFrom(a => a.Employee.Name))
               .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
              .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status.State == DocumentStatus.Unpaid ? "Approved" :
                    a.Status.State == DocumentStatus.Partial ? "Approved" :
                    a.Status.State == DocumentStatus.Paid ? "Approved" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.RequisitionDocNo, core => core.MapFrom(a => a.Requisition.DocNo));

            CreateMap<IssuanceLines, IssuanceLinesDto>()
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.WarehouseName, core => core.MapFrom(a => a.Warehouse.Name))
              .ForMember(dto => dto.PendingQuantity, core => core.MapFrom(a => a.Quantity));

            CreateMap<CreateIssuanceDto, IssuanceMaster>();
            CreateMap<CreateIssuanceLinesDto, IssuanceLines>();

            // Stock Mapping
            CreateMap<Stock, StockDto>()
                .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
                .ForMember(dto => dto.UnitOfMeasurement, core => core.MapFrom(a => a.Item.UnitOfMeasurement.Name))
                .ForMember(dto => dto.Category, core => core.MapFrom(a => a.Item.Category.Name));
            CreateMap<StockDto, Stock>();

            // GoodsReturnNote Mapping
            CreateMap<GoodsReturnNoteMaster, GoodsReturnNoteDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
               .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.GRNDocNo, core => core.MapFrom(a => a.GRN.DocNo));

            CreateMap<GoodsReturnNoteLines, GoodsReturnNoteLinesDto>()
              .ForMember(dto => dto.Warehouse, core => core.MapFrom(a => a.Warehouse.Name))
              .ForMember(dto => dto.Item, core => core.MapFrom(a => a.Item.ProductName));

            CreateMap<CreateGoodsReturnNoteDto, GoodsReturnNoteMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.GoodsReturnNoteLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.GoodsReturnNoteLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.GoodsReturnNoteLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateGoodsReturnNoteLinesDto, GoodsReturnNoteLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));

            // IssuanceReturn Mapping
            CreateMap<IssuanceReturnMaster, IssuanceReturnDto>()
              .ForMember(dto => dto.EmployeeName, core => core.MapFrom(a => a.Employee.Name))
               .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.IssuanceDocNo, core => core.MapFrom(a => a.Issuance.DocNo));

            CreateMap<IssuanceReturnLines, IssuanceReturnLinesDto>()
              .ForMember(dto => dto.Warehouse, core => core.MapFrom(a => a.Warehouse.Name))
              .ForMember(dto => dto.Item, core => core.MapFrom(a => a.Item.ProductName));

            CreateMap<CreateIssuanceReturnDto, IssuanceReturnMaster>();

            CreateMap<CreateIssuanceReturnLinesDto, IssuanceReturnLines>();
            CreateMap<CreateIssuanceLinesDto, IssuanceLines>();

            // Department Mapping
            CreateMap<Remark, RemarksDto>();
            CreateMap<RemarksDto, Remark>();
        }
    }
}
