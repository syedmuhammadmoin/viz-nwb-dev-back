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
               .ForMember(dto => dto.TotalDebit, core => core.MapFrom(a => a.JournalEntryLines.Sum(i => i.Debit)))
               .ForMember(dto => dto.TotalCredit, core => core.MapFrom(a => a.JournalEntryLines.Sum(i => i.Credit)));

            CreateMap<JournalEntryLines, JournalEntryLinesDto>()
              .ForMember(dto => dto.AccountName, core => core.MapFrom(a => a.Account.Name))
              .ForMember(dto => dto.BusinessPartnerName, core => core.MapFrom(a => a.BusinessPartner.Name))
              .ForMember(dto => dto.LocationName, core => core.MapFrom(a => a.Location.Name));

            CreateMap<CreateJournalEntryDto, JournalEntryMaster>()
               .ForMember(core => core.TotalDebit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Debit)))
               .ForMember(core => core.TotalCredit, dto => dto.MapFrom(a => a.JournalEntryLines.Sum(i => i.Credit))); ;



            CreateMap<CreateJournalEntryLinesDto, JournalEntryLines>();
        }
    }
}
