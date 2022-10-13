using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class Level4Specs : BaseSpecification<Level4>
    {
        public Level4Specs(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Level3);
            ApplyOrderByDescending(i => i.Id);
        }

        public Level4Specs()
        {
            AddInclude(i => i.Level3);
        }

        public Level4Specs(bool isBudget) : base(i => i.Level1_id == new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
                                                    || i.Level1_id == new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"))
        {
        }

        public Level4Specs(int id, bool isReceivable)
            : base(
                  isReceivable ?
                  (x => x.Level3_id == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
                  || x.Level3_id == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
                  || x.Level3_id == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
                  || x.Level3_id == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
                  || x.Level3_id == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
                  || x.Level3_id == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00"))
                  : (x => x.Level3_id == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00")
                  ))
        {
        }

        public Level4Specs(int getAll)
            : base(x => x.Level3_id != new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
                  && x.Level3_id != new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
                  && x.Level3_id != new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
                  && x.Level3_id != new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
                  && x.Level3_id != new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
                  && x.Level3_id != new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
                  && x.Level3_id != new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
        {
        }

        public Level4Specs(string code) : base (i => i.Code == code)
        {
            ApplyAsNoTracking();    
        }

        public Level4Specs(string code, Guid id) : base(i => i.Code == code && i.Id != id)
        {
            ApplyAsNoTracking();
        }
        public Level4Specs(bool isCOA, int id)
        {
            AddInclude(i => i.Level3);
            AddInclude("Level3.Level2");
            AddInclude(i => i.Level1);
            ApplyAsNoTracking();
        }
    }
}
