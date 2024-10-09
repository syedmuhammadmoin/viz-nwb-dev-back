using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class Level4Filter : PaginationFilter
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Level1Name { get; set; }
    }
}
