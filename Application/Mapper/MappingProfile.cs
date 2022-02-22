using Application.Contracts.DTOs;
using AutoMapper;
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
            // Client Mapping
            CreateMap<Client, ClientDto>();
            CreateMap<CreateClientDto, Client>();

            // Orgnization Mapping
            CreateMap<Organization, OrganizationDto>()
                .ForMember(dto => dto.ClientName, core => core.MapFrom(a => a.Client.Name));
            CreateMap<CreateOrganizationDto, Organization>();

            // Department Mapping
            CreateMap<Department, DeptDto>()
                .ForMember(dto => dto.OrgnizationName, core => core.MapFrom(a => a.Orgnization.Name));
            CreateMap<CreateDeptDto, Department>();

            // Warehouse Mapping
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dto => dto.DepartmentName, core => core.MapFrom(a => a.Department.Name));
            CreateMap<CreateWarehouseDto, Warehouse>();

            // Location Mapping
            CreateMap<Location, LocationDto>()
                .ForMember(dto => dto.WarehouseName, core => core.MapFrom(a => a.Warehouse.Name));
            CreateMap<CreateLocationDto, Location>();

            // Level4 Mapping
            CreateMap<Level4, Level4Dto>()
                .ForMember(dto => dto.Level3Name, core => core.MapFrom(a => a.Level3.Name));
            CreateMap<CreateLevel4Dto, Level4>();

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
                .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

            CreateMap<JournalEntryLines, JournalEntryLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.BusinessPartnerName, core => core.MapFrom(a => a.BusinessPartner.Name))
              .ForMember(dto => dto.LocationName, core => core.MapFrom(a => a.Location.Name));

            CreateMap<CreateJournalEntryDto, JournalEntryMaster>()
               .ForMember(core => core.TotalDebit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Debit)))
               .ForMember(core => core.TotalCredit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Credit))); ;

            CreateMap<CreateJournalEntryLinesDto, JournalEntryLines>();

            // Invoice Mapping
            CreateMap<InvoiceMaster, InvoiceDto>()
              .ForMember(dto => dto.CustomerName, core => core.MapFrom(a => a.Customer.Name))
                .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

            CreateMap<InvoiceLines, InvoiceLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.LocationName, core => core.MapFrom(a => a.Location.Name));

            CreateMap<CreateInvoiceDto, InvoiceMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.InvoiceLines.Sum(e => e.Quantity * e.Price)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.InvoiceLines.Sum(e => e.Quantity * e.Price * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.InvoiceLines.Sum(e => (e.Quantity * e.Price) + (e.Quantity * e.Price * e.Tax / 100))));

            CreateMap<CreateInvoiceLinesDto, InvoiceLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Price) + (a.Quantity * a.Price * a.Tax / 100)));

            // Bill Mapping
            CreateMap<BillMaster, BillDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name))
              .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));
            
            CreateMap<BillLines, BillLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.LocationName, core => core.MapFrom(a => a.Location.Name));

            CreateMap<CreateBillDto, BillMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.BillLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.BillLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateBillLinesDto, BillLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));

            // CreditNote Mapping
            CreateMap<CreditNoteMaster, CreditNoteDto>()
              .ForMember(dto => dto.CustomerName, core => core.MapFrom(a => a.Customer.Name))
              .ForMember(dto => dto.TransactionId, core => core.MapFrom(a => a.Transactions.Id));

            CreateMap<CreditNoteLines, CreditNoteLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.LocationName, core => core.MapFrom(a => a.Location.Name));

            CreateMap<CreateCreditNoteDto, CreditNoteMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.CreditNoteLines.Sum(e => e.Quantity * e.Price)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.CreditNoteLines.Sum(e => e.Quantity * e.Price * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.CreditNoteLines.Sum(e => (e.Quantity * e.Price) + (e.Quantity * e.Price * e.Tax / 100))));

            CreateMap<CreateCreditNoteLinesDto, CreditNoteLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Price) + (a.Quantity * a.Price * a.Tax / 100)));

            // DebitNote Mapping
            CreateMap<DebitNoteMaster, DebitNoteDto>()
              .ForMember(dto => dto.VendorName, core => core.MapFrom(a => a.Vendor.Name));

            CreateMap<DebitNoteLines, DebitNoteLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.ItemName, core => core.MapFrom(a => a.Item.ProductName))
              .ForMember(dto => dto.LocationName, core => core.MapFrom(a => a.Location.Name));

            CreateMap<CreateDebitNoteDto, DebitNoteMaster>()
               .ForMember(core => core.TotalBeforeTax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => e.Quantity * e.Cost)))
               .ForMember(core => core.TotalTax, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => e.Quantity * e.Cost * e.Tax / 100)))
               .ForMember(core => core.TotalAmount, dto => dto.MapFrom(a => a.DebitNoteLines.Sum(e => (e.Quantity * e.Cost) + (e.Quantity * e.Cost * e.Tax / 100))));

            CreateMap<CreateDebitNoteLinesDto, DebitNoteLines>()
               .ForMember(core => core.SubTotal, dto => dto.MapFrom(a => (a.Quantity * a.Cost) + (a.Quantity * a.Cost * a.Tax / 100)));
        }
    }
}
