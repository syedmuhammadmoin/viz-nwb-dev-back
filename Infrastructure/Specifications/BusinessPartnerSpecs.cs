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
    public class BusinessPartnerSpecs : BaseSpecification<BusinessPartner>
    {
        public BusinessPartnerSpecs(List<BusinessPartnerType?> businessPartnerTypes, BusinessPartnerFilter filter)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : "")
                && (businessPartnerTypes.Count() > 0 ? businessPartnerTypes.Contains(c.BusinessPartnerType):true))
        {
            var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
            ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
            ApplyOrderByDescending(i => i.Id);
            AddInclude(i=> i.AccountPayable);
            AddInclude(i=> i.AccountReceivable);
        }

        public BusinessPartnerSpecs() : base(x => x.BusinessPartnerType != BusinessPartnerType.Employee)
        {
            AddInclude(i => i.AccountPayable);
            AddInclude(i => i.AccountReceivable);
        }

        public BusinessPartnerSpecs(bool isNotEmployee) : base(x => isNotEmployee == true && x.BusinessPartnerType != BusinessPartnerType.Employee)
        {

        }
    }
}
