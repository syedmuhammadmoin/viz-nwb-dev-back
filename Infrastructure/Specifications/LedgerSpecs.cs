using Domain.Constants;
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
            AddInclude("Level4.Level1");
        }

        public LedgerSpecs(int transactionId) : base(i => i.TransactionId == transactionId
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
        }

        public LedgerSpecs(int transactionId, Guid level4Id, int? businessPartnerId, char sign, bool asNoTracking = false) 
        : base(i => i.TransactionId == transactionId
        && i.Level4_id == level4Id
        && i.BusinessPartnerId == businessPartnerId
        && i.Sign != sign
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
            if (asNoTracking)
                ApplyAsNoTracking();
        }

        public LedgerSpecs(Guid level4Id, int? businessPartnerId, char sign)
        : base(i => i.Level4_id == level4Id
        && i.BusinessPartnerId == businessPartnerId
        && i.Sign != sign
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            ApplyAsNoTracking();
        }
    }
}
