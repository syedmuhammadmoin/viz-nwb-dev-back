using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class BusinessPartnerFilter : PaginationFilter
    {
        public string Name { get; set; }
        public BusinessPartnerType? Type { get; set; }
    }
}
