using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface ILevel4Service : ICrudService<CreateLevel4Dto, Level4Dto, Guid, PaginationFilter>
    {
        Task<Response<List<Level4Dto>>> GetLevel4DropDown();
    }
}
