using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class LedgerSpecs : BaseSpecification<RecordLedger>
    {
        public LedgerSpecs()
        {
            AddInclude(i => i.Level4);
            AddInclude(i => i.Transactions);
            AddInclude(i => i.Campus);
            AddInclude(i => i.BusinessPartner);
            AddInclude(i => i.Warehouse);
        }
    }
}
