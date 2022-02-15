using Application.Contracts.DTOs;
using Application.Contracts.DTOs.Products;
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

            CreateMap<Level4, Level4Dto>()
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Name));
            CreateMap<CreateLevel4Dto, Level4>();

            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Name));
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<BusinessPartner, BusinessPartnerDto>()
                .ForMember(dto => dto.Name, core => core.MapFrom(a => a.Name));
            CreateMap<CreateBusinessPartnerDto, BusinessPartner>();

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.ProductName, core => core.MapFrom(a => a.ProductName));
            CreateMap<CreateProductDto, Product>();
        }
    }
}
