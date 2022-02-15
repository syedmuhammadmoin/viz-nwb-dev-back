using Application.Contracts.DTOs.Products;
using Application.Contracts.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IProductService : ICrudService<CreateProductDto, ProductDto, int, PaginationFilter>
    {

    }
}
