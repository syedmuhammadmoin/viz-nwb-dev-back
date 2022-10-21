using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class PayrollItemSpecs : BaseSpecification<PayrollItem>
    {
        public PayrollItemSpecs(List<CalculationType?> payrollItemType, List<PayrollType?> payrollType, PayrollItemFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : "")
            && c.ItemCode.Contains(filter.ItemCode != null ? filter.ItemCode : "")
            && (filter.IsActive != null ? c.IsActive == filter.IsActive : true)
            && (payrollItemType.Count() > 0 ? payrollItemType.Contains(c.PayrollItemType) : true)
            && (payrollType.Count() > 0 ? payrollType.Contains(c.PayrollType) : true)
            )
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                AddInclude(i => i.Account);
                ApplyOrderByDescending(i => i.Id);
            }
        }

        public PayrollItemSpecs()
        {
            AddInclude(i => i.Account);
        }

        public PayrollItemSpecs(bool isBasicPay) : base(x => isBasicPay ? x.PayrollType == PayrollType.BasicPay : false)
        {
        }
    }
}
