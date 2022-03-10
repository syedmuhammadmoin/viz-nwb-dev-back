using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IBillService : ICrudService<CreateBillDto, BillDto, int, PaginationFilter>
    {
    }
}
