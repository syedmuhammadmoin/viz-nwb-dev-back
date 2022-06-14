using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GetLedgerByIdSpecs : BaseSpecification<RecordLedger>
    {
        public GetLedgerByIdSpecs()
        {
            AddInclude(i => i.Transactions);
        }
    }
}
