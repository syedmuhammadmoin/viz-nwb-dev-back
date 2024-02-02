using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class FinanceAccountTypes
    {
        public static readonly Guid Assets = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00");
        public static readonly Guid Liability = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00");
        public static readonly Guid AccumulatedFund = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00");
        public static readonly Guid Revenue = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00");
        public static readonly Guid Expenses = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00");
    }
}
