using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class PaginationFilter
    {
        public int PageStart { get; set; }
        public int PageEnd { get; set; }
        public PaginationFilter()
        {
            this.PageStart = 0;
            this.PageEnd = 20;
        }
        public PaginationFilter(int pageStart, int pageEnd)
        {
            this.PageStart = pageStart;
            this.PageEnd = pageEnd;
        }
    }
}
