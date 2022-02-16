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
                .ForMember(dto => dto.levle3Name, core => core.MapFrom(a => a.Level3.Name));
            CreateMap<CreateLevel4Dto, Level4>();

            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Name));
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<BusinessPartner, BusinessPartnerDto>()
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Name));
            CreateMap<CreateBusinessPartnerDto, BusinessPartner>();

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.CategoryName, core => core.MapFrom(a => a.Category.Name));
            CreateMap<CreateProductDto, Product>();
        }
    }
}
