using Application.Contracts.DTOs;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;

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
            CreateMap<CreateCampusDto, Campus>()
                .ForMember(core => core.OrganizationId, dto => dto.MapFrom(a => 1));

            // Department Mapping
            CreateMap<Department, DepartmentDto>()
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name));
            CreateMap<CreateDepartmentDto, Department>();


            // Designation Mapping
            CreateMap<Designation, DesignationDto>();
            CreateMap<DesignationDto, Designation>();

            // Employee Mapping
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dto => dto.DepartmentName, core => core.MapFrom(a => a.Department.Name))
                .ForMember(dto => dto.DesignationName, core => core.MapFrom(a => a.Designation.Name))
                .ForMember(dto => dto.AccountPayableName, core => core.MapFrom(a => a.BusinessPartner.AccountPayable.Name))
                .ForMember(dto => dto.AccountPayableId, core => core.MapFrom(a => a.BusinessPartner.AccountPayableId))
                .ForMember(dto => dto.CampusId, core => core.MapFrom(a => a.Department.Campus.Id))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Department.Campus.Name));

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
                .ForMember(dto => dto.Level3Name, core => core.MapFrom(a => a.Level3.Name))
                .ForMember(dto => dto.EditableName, core => core.MapFrom(a => a.Name))
                .ForMember(dto => dto.Name, core => core.MapFrom(a => ($"{a.Code}-{a.Name}")));
            CreateMap<CreateLevel4Dto, Level4>()
                .ForMember(core => core.AccountType, dto => dto.MapFrom(a => AccountType.UserDefined))
                .ForMember(core => core.Name,dto => dto.MapFrom(a => a.EditableName));
                

            // Level3 Mapping
            CreateMap<Level3, Level3DropDownDto>()
                .ForMember(dto => dto.Level1Name, core => core.MapFrom(a => a.Level2.Level1.Name));

            // Level1 Mapping
            CreateMap<Level1, Level1Dto>()
                .ForMember(dto => dto.children, core => core.MapFrom(a => a.Level2))
                .ForMember(dto => dto.Name, core => core.MapFrom(a => ($"{a.Code}-{a.Name}")));
            CreateMap<Level2, Level2Dto>()
                .ForMember(dto => dto.children, core => core.MapFrom(a => a.Level3))
                .ForMember(dto => dto.Name, core => core.MapFrom(a => ($"{a.Code}-{a.Name}")));
            CreateMap<Level3, Level3Dto>()
                .ForMember(dto => dto.children, core => core.MapFrom(a => a.Level4))
                .ForMember(dto => dto.Name, core => core.MapFrom(a => ($"{a.Code}-{a.Name}")));

            // Category Mapping
            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.InventoryAccount, core => core.MapFrom(a => a.InventoryAccount.Name))
                .ForMember(dto => dto.RevenueAccount, core => core.MapFrom(a => a.RevenueAccount.Name))
                .ForMember(dto => dto.CostAccount, core => core.MapFrom(a => a.CostAccount.Name))
                .ForMember(dto => dto.DepreciationModel, core => core.MapFrom(a => a.DepreciationModel.ModelName));
            CreateMap<CreateCategoryDto, Category>();

            // BusinessPartner Mapping
            CreateMap<BusinessPartner, BusinessPartnerDto>()
                .ForMember(dto => dto.AccountReceivable, core => core.MapFrom(a => a.AccountReceivable.Name))
                .ForMember(dto => dto.AccountPayable, core => core.MapFrom(a => a.AccountPayable.Name));
            CreateMap<CreateBusinessPartnerDto, BusinessPartner>();

            // Product Mapping
            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.CategoryName, core => core.MapFrom(a => a.Category.Name))
                .ForMember(dto => dto.IsFixedAsset, core => core.MapFrom(a => a.Category.IsFixedAsset))
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
               .ForMember(core => core.TotalCredit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Credit)));

            CreateMap<CreateJournalEntryLinesDto, JournalEntryLines>();

            // PettyCash Mapping
            CreateMap<PettyCashMaster, PettyCashDto>()
                .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
                .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));

            CreateMap<PettyCashLines, PettyCashLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.BusinessPartnerName, core => core.MapFrom(a => a.BusinessPartner.Name));

            CreateMap<CreatePettyCashDto, PettyCashMaster>()
               .ForMember(core => core.TotalDebit, dto => dto.MapFrom(a => a.PettyCashLines.Sum(i => i.Debit)))
               .ForMember(core => core.TotalCredit, dto => dto.MapFrom(a => a.PettyCashLines.Sum(i => i.Credit)));

            CreateMap<CreatePettyCashLinesDto, PettyCashLines>();

            // Invoice Mapping
            CreateMap<InvoiceMaster, InvoiceDto>()
              .ForMember(dto => dto.CustomerName, core => core.MapFrom(a => a.Customer.Name))
              .ForMember(dto => dto.CustomerAddress, core => core.MapFrom(a => a.Customer.Address))
              .ForMember(dto => dto.SalesTaxId, core => core.MapFrom(a => a.Customer.SalesTaxId))
              .ForMember(dto => dto.IncomeTaxId, core => core.MapFrom(a => a.Customer.IncomeTaxId))
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
              .ForMember(dto => dto.CustomerAddress, core => core.MapFrom(a => a.Customer.Address))
              .ForMember(dto => dto.SalesTaxId, core => core.MapFrom(a => a.Customer.SalesTaxId))
              .ForMember(dto => dto.IncomeTaxId, core => core.MapFrom(a => a.Customer.IncomeTaxId))
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
              .ForMember(dto => dto.VendorAddress, core => core.MapFrom(a => a.Vendor.Address))
              .ForMember(dto => dto.SalesTaxId, core => core.MapFrom(a => a.Vendor.SalesTaxId))
              .ForMember(dto => dto.IncomeTaxId, core => core.MapFrom(a => a.Vendor.IncomeTaxId))
              .ForMember(dto => dto.PayableAccountName, core => core.MapFrom(a => a.PayableAccount.Name))
              .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State == DocumentStatus.Unpaid ? "Unpaid" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.GRNDocNo, core => core.MapFrom(a => a.GRN.DocNo));

            CreateMap<BillLines, BillLinesDto>()
              .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));

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
              .ForMember(dto => dto.VendorAddress, core => core.MapFrom(a => a.Vendor.Address))
              .ForMember(dto => dto.SalesTaxId, core => core.MapFrom(a => a.Vendor.SalesTaxId))
              .ForMember(dto => dto.IncomeTaxId, core => core.MapFrom(a => a.Vendor.IncomeTaxId))
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
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => (e.Quantity * e.Cost * e.Tax / 100) + e.AnyOtherTax)))
               .ForMember(core => core.OtherTax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => e.AnyOtherTax)))
               .ForMember(core => core.Tax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => (e.Quantity * e.Cost * e.Tax / 100))))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100) + (e.AnyOtherTax))));

            CreateMap<CreateDebitNoteLinesDto, DebitNoteLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100) + (a.AnyOtherTax)));

            //Payment Mapping
            CreateMap<Payment, PaymentDto>()
                .ForMember(dto => dto.BusinessPartnerName, core => core.MapFrom(a => a.BusinessPartner.Name))
                .ForMember(dto => dto.BusinessPartnerAddress, core => core.MapFrom(a => a.BusinessPartner.Address))
                .ForMember(dto => dto.BusinessPartnerMobile, core => core.MapFrom(a => a.BusinessPartner.Mobile))
                .ForMember(dto => dto.SalesTaxId, core => core.MapFrom(a => a.BusinessPartner.SalesTaxId))
                .ForMember(dto => dto.IncomeTaxId, core => core.MapFrom(a => a.BusinessPartner.IncomeTaxId))
                .ForMember(dto => dto.BusinessPartnerMobile, core => core.MapFrom(a => a.BusinessPartner.Mobile))
                .ForMember(dto => dto.PaymentRegisterName, core => core.MapFrom(a => a.PaymentRegister.Name))
                .ForMember(dto => dto.DeductionAccountName, core => core.MapFrom(a => a.DeductionAccount.Name))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
                .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
                .ForMember(dto => dto.SalesTaxInAmount, core => core.MapFrom(a => (a.GrossPayment * a.SalesTax) / 100))
                .ForMember(dto => dto.IncomeTaxInAmount, core => core.MapFrom(a => (a.GrossPayment * a.IncomeTax) / 100))
                .ForMember(dto => dto.SRBTaxInAmount, core => core.MapFrom(a => (a.GrossPayment * a.SRBTax) / 100))
              .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.BankReconStatus == DocumentStatus.Unreconciled ? "In Payment" :
                    a.BankReconStatus == DocumentStatus.Partial ? "Partial Reconciled" :
                    a.BankReconStatus == DocumentStatus.Reconciled ? "Reconciled" : a.Status.Status));

            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(core => core.NetPayment, dto => dto.MapFrom(a => (a.GrossPayment - ((a.GrossPayment * a.IncomeTax) / 100) - ((a.GrossPayment * a.SalesTax) / 100) - ((a.GrossPayment * a.SRBTax) / 100) - a.Deduction)));

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
                .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
                .ForMember(dto => dto.AccountCode, core => core.MapFrom(a => a.ChAccount.Code));
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
            CreateMap<BudgetMaster, BudgetDto>()
              .ForMember(dto => dto.CampusName, core => core.MapFrom(a => a.Campus.Name))
            .ForMember(dto => dto.Status, core => core.MapFrom(d => d.Status.State == DocumentStatus.Unpaid ? "Approved" : d.Status.Status))
               .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<BudgetLines, BudgetLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreateBudgetDto, BudgetMaster>();
            CreateMap<CreateBudgetLinesDto, BudgetLines>();


            CreateMap<BudgetMaster, CreateBudgetDto>();
            CreateMap<BudgetLines, CreateBudgetLinesDto>();

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
              .ForMember(dto => dto.PendingQuantity, core => core.MapFrom(a => a.Quantity))
              .ForMember(dto => dto.SubTotal, core => core.MapFrom(a => a.Quantity * a.PurchasePrice));

            CreateMap<RequisitionMaster, RequisitionDropDownDto>();

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
              .ForMember(dto => dto.PendingQuantity, core => core.MapFrom(a => a.Quantity))
              .ForMember(dto => dto.IsFixedAsset, core => core.MapFrom(a => a.Item.ProductType == ProductType.FixedAsset ? true : false))
              ;

            CreateMap<CreateGRNDto, GRNMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.GRNLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.GRNLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.GRNLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateGRNLinesDto, GRNLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));

            // EstimatedBudget Mapping
            CreateMap<EstimatedBudgetMaster, EstimatedBudgetDto>()
                .ForMember(dto => dto.From, core => core.MapFrom(a => a.PreviousBudget.From))
                .ForMember(dto => dto.To, core => core.MapFrom(a => a.PreviousBudget.To))
                .ForMember(dto => dto.CampusId, core => core.MapFrom(a => a.PreviousBudget.CampusId))
               .ForMember(dto => dto.Status, core => core.MapFrom(d => d.Status.State == DocumentStatus.Unpaid ? "Approved" : d.Status.Status))
               .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));
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
              .ForMember(dto => dto.Employee, core => core.MapFrom(a => a.Name))
              .ForMember(dto => dto.BusinessPartnerId, core => core.MapFrom(a => a.Employee.BusinessPartnerId))
              .ForMember(dto => dto.AccountPayable, core => core.MapFrom(a => a.AccountPayable.Name))
              .ForMember(dto => dto.Department, core => core.MapFrom(a => a.Department.Name))
              .ForMember(dto => dto.Designation, core => core.MapFrom(a => a.Designation.Name))
              .ForMember(dto => dto.Campus, core => core.MapFrom(a => a.Campus.Name))
              .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State == DocumentStatus.Unpaid ? "Unpaid" : a.Status.Status))
              .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State))
              .ForMember(dto => dto.AbsentDays, core => core.MapFrom(a => a.WorkingDays - a.PresentDays - a.LeaveDays));

            CreateMap<PayrollTransactionLines, PayrollTransactionLinesDto>()
              .ForMember(dto => dto.PayrollItem, core => core.MapFrom(a => a.PayrollItem.Name))
              .ForMember(dto => dto.Account, core => core.MapFrom(a => a.Account.Name));

            CreateMap<CreatePayrollTransactionDto, PayrollTransactionMaster>();

            // Tax Mapping
            CreateMap<Taxes, TaxDto>()
                .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name));
            CreateMap<UpdateTaxDto, Taxes>();
            CreateMap<CreateTaxDto, Taxes>();
            
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



            CreateMap<IssuanceMaster, CreateIssuanceDto>();
            CreateMap<IssuanceLines, CreateIssuanceLinesDto>();
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

            // Remarks Mapping
            CreateMap<Remark, RemarksDto>();
            CreateMap<RemarksDto, Remark>();

            // FileUpload Mapping
            CreateMap<FileUpload, FileUploadDto>();
            CreateMap<FileUploadDto, FileUpload>();

            // User Mapping
            CreateMap<User, UsersListDto>()
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Employee.Name));

            //Request 
            CreateMap<RequestMaster, RequestDto>()
                .ForMember(dto => dto.EmployeeName, core => core.MapFrom(a => a.Employee.Name))
                .ForMember(dto => dto.Campus, core => core.MapFrom(a => a.Campus.Name))
                .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status.State == DocumentStatus.Unpaid ? "Approved" : a.Status.Status))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<RequestLines, RequestLinesDto>();
            CreateMap<CreateRequestDto, RequestMaster>();
            CreateMap<CreateRequestLinesDto, RequestLines>();

            // Bid Evaluation 
            CreateMap<BidEvaluationMaster, BidEvaluationDto>()
                .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.State == DocumentStatus.Draft ? "Draft" :
                    a.State == DocumentStatus.Submitted ? "Submitted" : "N/A"));

            CreateMap<BidEvaluationLines, BidEvaluationLinesDto>();
            CreateMap<CreateBidEvaluationLinesDto, BidEvaluationLines>();
            CreateMap<CreateBidEvaluationDto, BidEvaluationMaster>();

            //Quotation 
            CreateMap<QuotationMaster, QuotationDto>()
                .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
                .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status.State == DocumentStatus.Paid ? "Awarded" : a.Status.Status))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<QuotationLines, QuotationLinesDto>()
                .ForMember(dto => dto.ItemId, core => core.MapFrom(a => a.ItemId == null ? null : a.ItemId))
                .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));
            CreateMap<CreateQuotationDto, QuotationMaster>();
            CreateMap<CreateQuotationLinesDto, QuotationLines>();

            //CallForQuotation 
            CreateMap<CallForQuotationMaster, CallForQuotationDto>()
                .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
                .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.State == DocumentStatus.Draft ? "Draft" :
                    a.State == DocumentStatus.Submitted ? "Submitted" : "N/A"));

            CreateMap<CallForQuotationLines, CallForQuotationLinesDto>()
                .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName));
            CreateMap<CreateCallForQuotationDto, CallForQuotationMaster>();
            CreateMap<CreateCallForQuotationLinesDto, CallForQuotationLines>();

            //QuotationComparative
            CreateMap<QuotationComparativeMaster, QuotationComparativeDto>()
                .ForMember(dto => dto.Quotations, core => core.MapFrom(a => a.Quotations))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status))
                .ForMember(dto => dto.Status, core => core.MapFrom(
                    a => a.Status == DocumentStatus.Draft ? "Draft" :
                    a.Status == DocumentStatus.Submitted ? "Submitted" :
                    a.Status == DocumentStatus.Paid ? "Awarded" : "N/A"));

            //DepreciationModel
            CreateMap<DepreciationModel, DepreciationModelDto>()
                .ForMember(dto => dto.AssetAccount, core => core.MapFrom(a => a.AssetAccount.Name))
                .ForMember(dto => dto.DepreciationExpense, core => core.MapFrom(d => d.DepreciationExpense.Name))
                .ForMember(dto => dto.AccumulatedDepreciation, core => core.MapFrom(a => a.AccumulatedDepreciation.Name));
            CreateMap<CreateDepreciationModelDto, DepreciationModel>();

            //FixedAsset
            CreateMap<FixedAsset, FixedAssetDto>()
                .ForMember(dto => dto.Product, core => core.MapFrom(a => a.Product.ProductName))
                .ForMember(dto => dto.Warehouse, core => core.MapFrom(a => a.Warehouse.Name))
                .ForMember(dto => dto.DepreciationModel, core => core.MapFrom(d => d.DepreciationModel.ModelName))
                .ForMember(dto => dto.AssetAccount, core => core.MapFrom(a => a.AssetAccount.Name))
                .ForMember(dto => dto.EmployeeId, core => core.MapFrom(a => a.EmployeeId))
                .ForMember(dto => dto.Employee, core => core.MapFrom(a => a.Employee.Name))
                .ForMember(dto => dto.DepreciationExpense, core => core.MapFrom(d => d.DepreciationExpense.Name))
                .ForMember(dto => dto.AccumulatedDepreciation, core => core.MapFrom(a => a.AccumulatedDepreciation.Name))
                .ForMember(dto => dto.Status, core => core.MapFrom(a => a.StatusId == 3 ? "Approved"
                : a.Status.Status))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));                

            CreateMap<FixedAssetLines, FixedAssetLinesDto>();

            CreateMap<DepreciationRegister, DepreciationRegisterDto>();

            CreateMap<CreateDepreciationRegisterDto, DepreciationRegister>();

            CreateMap<CreateFixedAssetDto, FixedAsset>();

            CreateMap<FixedAssetLinesDto, FixedAssetLines>();

            CreateMap<UpdateFixedAssetDto, FixedAsset>();

            CreateMap<UpdateSalvageValueDto, FixedAsset>();

            CreateMap<CWIP, CreateFixedAssetDto>()
                .ForMember(fixedAsset => fixedAsset.Doctype, cwip => cwip.MapFrom(d => DocType.CWIP))
                .ForMember(fixedAsset => fixedAsset.DocId, cwip => cwip.MapFrom(d => d.Id));

            //CWIP
            CreateMap<CWIP, CWIPDto>()
                .ForMember(dto => dto.CWIPAccount, core => core.MapFrom(d => d.CWIPAccount.Name))
                .ForMember(dto => dto.Warehouse, core => core.MapFrom(d => d.Warehouse.Name))
                .ForMember(dto => dto.DepreciationModel, core => core.MapFrom(d => d.DepreciationModel.ModelName))
                .ForMember(dto => dto.AssetAccount, core => core.MapFrom(d => d.AssetAccount.Name))
                .ForMember(dto => dto.DepreciationExpense, core => core.MapFrom(d => d.DepreciationExpense.Name))
                .ForMember(dto => dto.AccumulatedDepreciation, core => core.MapFrom(d => d.AccumulatedDepreciation.Name))
                .ForMember(dto => dto.Product, core => core.MapFrom(d => d.Product.ProductName))
                .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.Status))
                .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<CreateCWIPDto, CWIP>();

            //Disposal
            CreateMap<Disposal, DisposalDto>()
               .ForMember(dto => dto.FixedAsset, core => core.MapFrom(d => d.FixedAsset.Name))
               .ForMember(dto => dto.Product, core => core.MapFrom(d => d.Product.ProductName))
               .ForMember(dto => dto.AccumulatedDepreciation, core => core.MapFrom(d => d.AccumulatedDepreciation.Name))
               .ForMember(dto => dto.Warehouse, core => core.MapFrom(d => d.Warehouse.Name))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State))
               .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<CreateDisposalDto, Disposal>()
                .ForMember(core => core.BookValue, dto => dto.MapFrom(a => 0));

            //BudgetReappropriation
            CreateMap<BudgetReappropriationMaster, BudgetReappropriationDto>()
               .ForMember(dto => dto.Budget, core => core.MapFrom(d => d.Budget.BudgetName))
               .ForMember(dto => dto.Status, core => core.MapFrom(d => d.Status.Status))
               .ForMember(dto => dto.Status, core => core.MapFrom(a => a.Status.State))
               .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<CreateBudgetReappropriationDto, BudgetReappropriationMaster>();
            CreateMap<BudgetReappropriationLines, BudgetReappropriationLinesDto>()
                   .ForMember(dto => dto.Level4, core => core.MapFrom(d => d.Level4.Name));
            CreateMap<CreateBudgetReappropriationLinesDto, BudgetReappropriationLines>();

            //DepreciationAdjustment
            CreateMap<DepreciationAdjustmentMaster, DepreciationAdjustmentDto>()
               .ForMember(dto => dto.Status, core => core.MapFrom(d => d.Status.Status))
               .ForMember(dto => dto.State, core => core.MapFrom(a => a.Status.State));

            CreateMap<DepreciationAdjustmentLines, DepreciationAdjustmentLinesDto>()
                   .ForMember(dto => dto.Level4, core => core.MapFrom(d => d.Level4.Name))
                   .ForMember(dto => dto.FixedAsset, core => core.MapFrom(d => d.FixedAsset.Name));

            CreateMap<CreateDepreciationAdjustmentDto, DepreciationAdjustmentMaster>();
            CreateMap<CreateDepreciationAdjustmentLinesDto, DepreciationAdjustmentLines>();

            //AcademicDepartment
            CreateMap<AcademicDepartment, AcademicDepartmentDto>()
                   .ForMember(dto => dto.Faculty, core => core.MapFrom(d => d.Faculty.Name));
            CreateMap<CreateAcademicDepartmentDto, AcademicDepartment>();

            //Faculty
            CreateMap<Faculty, FacultyDto>();
            CreateMap<CreateFacultyDto, Faculty>();

            //Degree
            CreateMap<DegreeDto, Degree>();
            CreateMap<Degree, DegreeDto>();

            //Program
            CreateMap<Program, ProgramDto>()
                   .ForMember(dto => dto.Degree, core => core.MapFrom(d => d.Degree.Name))
                   .ForMember(dto => dto.AcademicDepartment, core => core.MapFrom(d => d.AcademicDepartment.Name));
            CreateMap<CreateProgramDto, Program>();

            CreateMap<ProgramSemesterCourse, SemesterCourseDto>()
                   .ForMember(dto => dto.Course, core => core.MapFrom(d => d.Course.Name));
            CreateMap<CreateSemesterCourseDto, ProgramSemesterCourse>();

            //Semester
            CreateMap<SemesterDto, Semester>();
            CreateMap<Semester, SemesterDto>();

            //Course
            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();

            //Qualification
            CreateMap<QualificationDto, Qualification>();
            CreateMap<Qualification, QualificationDto>();

            //Subject
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<Subject, SubjectDto>()
                .ForMember(dto => dto.Qualification, core => core.MapFrom(d => d.Qualification.Name)); ;


            //FeeItem
            CreateMap<FeeItem, FeeItemDto>()
                   .ForMember(dto => dto.Account, core => core.MapFrom(d => d.Account.Name));
            CreateMap<CreateFeeItemDto, FeeItem>();

            //Country
            CreateMap<CountryDto, Country>();
            CreateMap<Country, CountryDto>();

            //State
            CreateMap<State, StateDto>()
                   .ForMember(dto => dto.Country, core => core.MapFrom(d => d.Country.Name));
            CreateMap<CreateStateDto, State>();

            //City
            CreateMap<City, CityDto>()
                   .ForMember(dto => dto.State, core => core.MapFrom(d => d.State.Name))
                   .ForMember(dto => dto.Country, core => core.MapFrom(d => d.State.Country.Name));
            CreateMap<CreateCityDto, City>();

            //District
            CreateMap<District, DistrictDto>()
                   .ForMember(dto => dto.City, core => core.MapFrom(d => d.City.Name));
            CreateMap<CreateDistrictDto, District>();

            //Domicile
            CreateMap<Domicile, DomicileDto>()
                   .ForMember(dto => dto.District, core => core.MapFrom(d => d.District.Name));
            CreateMap<CreateDomicileDto, Domicile>();

            //Shift
            CreateMap<ShiftDto, Shift>();
            CreateMap<Shift, ShiftDto>();

            //Batch
            CreateMap<BatchMaster, BatchDto>()
               .ForMember(dto => dto.Semester, core => core.MapFrom(a => a.Semester.Name))
               .ForMember(dto => dto.Campus, core => core.MapFrom(d => d.Campus.Name))
               .ForMember(dto => dto.Shift, core => core.MapFrom(d => d.Shift.Name));

            CreateMap<BatchLines, BatchLinesDto>()
                   .ForMember(dto => dto.Program, core => core.MapFrom(d => d.Program.Name));

            CreateMap<CreateBatchDto, BatchMaster>();
            CreateMap<CreateBatchLinesDto, BatchLines>();

            //AdmissionCriteria
            CreateMap<AdmissionCriteria, AdmissionCriteriaDto>()
                   .ForMember(dto => dto.Program, core => core.MapFrom(d => d.Program.Name))
                   .ForMember(dto => dto.Qualification, core => core.MapFrom(d => d.Qualification.Name))
                   .ForMember(dto => dto.Subject, core => core.MapFrom(d => d.Subject.Name));
            CreateMap<CreateAdmissionCriteriaDto, AdmissionCriteria>();

            //Applicant
            CreateMap<Applicant, ApplicantDto>()
                   .ForMember(dto => dto.PlaceOfBirth, core => core.MapFrom(d => d.PlaceOfBirth.Name))
                   .ForMember(dto => dto.Domicile, core => core.MapFrom(d => d.Domicile.Name))
                   .ForMember(dto => dto.Nationality, core => core.MapFrom(d => d.Nationality.Name));
            CreateMap<CreateApplicantDto, Applicant>();
            CreateMap<RegisterApplicantDto, Applicant>();

            CreateMap<ApplicantQualification, ApplicantQualificationDto>()
                  .ForMember(dto => dto.Qualification, core => core.MapFrom(d => d.Qualification.Name))
                  .ForMember(dto => dto.Subject, core => core.MapFrom(d => d.Subject.Name));

            CreateMap<ApplicantRelative, ApplicantRelativeDto>();

            //ProgramChallanTemplate
            CreateMap<ProgramChallanTemplateMaster, ProgramChallanTemplateDto>()
               .ForMember(dto => dto.Program, core => core.MapFrom(a => a.Semester.Name))
               .ForMember(dto => dto.Campus, core => core.MapFrom(d => d.Campus.Name))
               .ForMember(dto => dto.Shift, core => core.MapFrom(d => d.Shift.Name))
               .ForMember(dto => dto.Semester, core => core.MapFrom(a => a.Semester.Name))
               .ForMember(dto => dto.BankAccount, core => core.MapFrom(a => a.BankAccount.Name));

            //Journals
            CreateMap<Journal, JournalDto>()
               .ForMember(dto => dto.SuspenseAccount, core => core.MapFrom(a => a.SuspenseAccount.Name))
               .ForMember(dto => dto.AccountNumber, core => core.MapFrom(a => a.AccountNumber.Name))
               .ForMember(dto => dto.CashAccount, core => core.MapFrom(a => a.CashAccount.Name))
               .ForMember(dto => dto.DefaultAccount, core => core.MapFrom(a => a.DefaultAccount.Name))
               .ForMember(dto => dto.LossAccount, core => core.MapFrom(a => a.LossAccount.Name))
               .ForMember(dto => dto.ProfitAccount, core => core.MapFrom(a => a.ProfitAccount.Name))
               .ForMember(dto => dto.BankName, core => core.MapFrom(a => a.BankName));
            CreateMap<CreateJournalDto, Journal>();
           

            CreateMap<ProgramChallanTemplateLines, ProgramChallanTemplateLinesDto>()
                   .ForMember(dto => dto.FeeItem, core => core.MapFrom(d => d.FeeItem.Name));

            CreateMap<CreateProgramChallanTemplateDto, ProgramChallanTemplateMaster>();
            CreateMap<CreateProgramChallanTemplateLinesDto, ProgramChallanTemplateLines>();

            CreateMap<PayrollResult, PayrollResultDto>();

            CreateMap<PayrollTransactionMaster, UpdateEmployeeTransactionDto>();
            CreateMap<UpdateEmployeeTransactionDto, PayrollTransactionMaster>();
           

        }
    }
}
