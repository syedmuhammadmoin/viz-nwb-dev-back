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
                .ForMember(dto => dto.DepartmentName, core => core.MapFrom(a => a.Designation.Name));

            CreateMap<CreateEmployeeDto, Employee>();

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
                .ForMember(dto => dto.CategoryName, core => core.MapFrom(a => a.Category.Name));
            CreateMap<CreateProductDto, Product>();

            // JournalEntry Mapping
            CreateMap<JournalEntryMaster, JournalEntryDto>()
                 .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
             .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

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
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
                .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

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
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
             .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

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
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

            CreateMap<BillLines, BillLinesDto>()
              .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));

            CreateMap<CreateBillDto, BillMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.BillLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateBillLinesDto, BillLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));


            // DebitNote Mapping
            CreateMap<DebitNoteMaster, DebitNoteDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
              .ForMember(dto => dto.PayableAccountName, core => core.MapFrom(a => a.PayableAccount.Name))
              .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
             .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

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
                 .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
             .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(core => core.NetPayment, dto => dto.MapFrom(a => (a.GrossPayment - a.Discount - a.IncomeTax - a.SalesTax)));

            // CashAccount Mapping
            CreateMap<CashAccount, CashAccountDto>()
                .ForMember(dto => dto.ChAccountName, core => core.MapFrom(a => a.ChAccount.Name))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
                .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));
            CreateMap<CreateCashAccountDto, CashAccount>();
            CreateMap<UpdateCashAccountDto, CashAccount>();

            // BankAccount Mapping
            CreateMap<BankAccount, BankAccountDto>()
                .ForMember(dto => dto.ChAccountName, core => core.MapFrom(a => a.ChAccount.Name))
                .ForMember(dto => dto.ClearingAccount, core => core.MapFrom(a => a.ClearingAccount.Name))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
                .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));
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
            CreateMap<CreateWorkFlowStatusDto, WorkFlowStatus>();

            // TransactionRecon Mapping
            CreateMap<CreateTransactionReconcileDto, TransactionReconcile>();

            // Budget Mapping
            CreateMap<BudgetMaster, BudgetDto>();
            CreateMap<BudgetLines, BudgetLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreateBudgetDto, BudgetMaster>();
            CreateMap<CreateBudgetLinesDto, BudgetLines>();

        }
    }
}
