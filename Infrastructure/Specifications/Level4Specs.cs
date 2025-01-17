﻿using Application.Contracts.Filters;
using Domain.Constants;
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
        public Level4Specs(Level4Filter filter) : base(c => c.Name.Contains(filter.Name != null ? filter.Name : "")
                && (c.Code.Contains(filter.Code != null ? filter.Code : ""))
                 && (c.Level3.Level2.Level1.Name.Contains(filter.Level1Name != null ? filter.Level1Name : "")))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            AddInclude(i => i.Level3);
            AddInclude(i => i.Level3.Level2);
            AddInclude(i => i.Level3.Level2.Level1);
            ApplyOrderByDescending(i => i.Id);
        }

        public Level4Specs()
        {
            AddInclude(i => i.Level3);
        }

        public Level4Specs(bool isReceivable, int orgId)
           : base(
                 isReceivable ?
                 (x => x.Level3_id == "12100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}")
                 : (x => x.Level3_id == "22100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}"))
        {
        }

        public Level4Specs(int orgId)
            : base(x => x.Level3_id != "12100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}"
            && x.Level3_id != "22100000-5566-7788-99AA-BBCCDDEEFF00" + $"-{orgId}")
        {
        }

        //public Level4Specs(bool isBudget) : base(i => i.Level1_id == new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
        //  || i.Level1_id == new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"))
        //{
        //}

        //public Level4Specs(int id, bool isReceivable)
        //    : base(
        //          isReceivable ?
        //          (x => x.Level3_id == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00"))
        //          : (x => x.Level3_id == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00")
        //          ))
        //{
        //}

        //public Level4Specs(int id, Guid Level4)
        //   : base(
        //         (x => x.Level3_id == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
        //         || x.Level3_id == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
        //         || x.Level3_id == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
        //         || x.Level3_id == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
        //         || x.Level3_id == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
        //         || x.Level3_id == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
        //         || x.Level3_id == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00")
        //         ))

        //{
        //}

        //public Level4Specs(int getAll)
        //    : base(x => x.Level3_id != new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
        //          && x.Level3_id != new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
        //          && x.Level3_id != new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
        //          && x.Level3_id != new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
        //          && x.Level3_id != new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
        //          && x.Level3_id != new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
        //          && x.Level3_id != new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00")
        //    )
        //{
        //}

        public Level4Specs(string code) : base(i => i.Code == code)
        {
        }

        public Level4Specs(string code, string id) : base(i => i.Code == code && i.Id != id)
        {
        }

        public Level4Specs(bool isCOA, int id, int NeedtoBeFixed)
        {
            AddInclude(i => i.Level3);
            AddInclude("Level3.Level2");
            AddInclude(i => i.Level1);
        }

        //public Level4Specs(Guid Level3Id)
        //        : base(x => x.Level3_id != new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id != new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id != new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id != new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id != new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id != new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
        //          || x.Level3_id != new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
        //{

        //}

        //public Level4Specs(string assetAccount, string l)
        //        : base(x => x.Level3.Level2_id == new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"))
        //{
        //    AddInclude(i => i.Level3);
        //}
        //public Level4Specs(string libilatyAccount, string l ,int id)
        //       : base(x => x.Level3.Level2_id == new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"))
        //{
        //    AddInclude(i => i.Level3);
        //}
    }
}
