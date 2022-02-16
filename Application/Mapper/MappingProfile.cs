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
            CreateMap<Client, ClientDto>();
            CreateMap<CreateClientDto, Client>();

            CreateMap<Organization, OrganizationDto>()
                .ForMember(dto => dto.ClientName, core => core.MapFrom(a => a.Client.Name));
            CreateMap<CreateOrganizationDto, Organization>();

            CreateMap<Department, DeptDto>()
                .ForMember(dto => dto.OrgnizationName, core => core.MapFrom(a => a.Orgnization.Name));
            CreateMap<CreateDeptDto, Department>();

            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dto => dto.DepartmentName, core => core.MapFrom(a => a.Department.Name));
            CreateMap<CreateWarehouseDto, Warehouse>();

            CreateMap<Location, LocationDto>()
                .ForMember(dto => dto.WarehouseName, core => core.MapFrom(a => a.Warehouse.Name));
            CreateMap<CreateLocationDto, Location>();

            CreateMap<Level4, Level4Dto>()
                .ForMember(dto => dto.Level3Name, core => core.MapFrom(a => a.Level3.Name));
            CreateMap<CreateLevel4Dto, Level4>();

            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.InventoryAccount, core => core.MapFrom(a => a.InventoryAccount.Name))
                .ForMember(dto => dto.RevenueAccount, core => core.MapFrom(a => a.RevenueAccount.Name))
                .ForMember(dto => dto.CostAccount, core => core.MapFrom(a => a.CostAccount.Name));
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<BusinessPartner, BusinessPartnerDto>()
                .ForMember(dto => dto.AccountReceivable, core => core.MapFrom(a => a.AccountReceivable.Name))
                .ForMember(dto => dto.AccountPayable, core => core.MapFrom(a => a.AccountPayable.Name));
            CreateMap<CreateBusinessPartnerDto, BusinessPartner>();

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.CategoryName, core => core.MapFrom(a => a.Category.Name));
            CreateMap<CreateProductDto, Product>();
        }
    }
}
