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
        public LedgerSpecs(string AccountID) : base(i => i.Level4_id== AccountID)
        {
            
            AddInclude(i => i.Level4);
            AddInclude("Level4.Level1");

        }
        public LedgerSpecs(string AccountId1, string AccountId2) : base(i => (i.Level4.Level1_id== AccountId1|| i.Level4.Level1_id == AccountId2) && i.TransactionDate >= DateTime.Now.AddMonths(-12))
        {
                    AddInclude(i => i.Level4);
                    AddInclude("Level4.Level1");
        }
        public LedgerSpecs(string AccountId1, string AccountId2,string NeedTobeFixed ) : base(i => (i.Level4.Level1_id == AccountId1 || i.Level4.Level1_id == AccountId2))
        {
            
            AddInclude("Level4.Level1");
            AddInclude("Level4.Level3");
            AddInclude("Level4.Level3.Level2");
        }
        //public LedgerSpecs(string NeedTobeFixed) : base(i => (i.Level4.paymen))
        //{

        //    AddInclude("Level4.Payment");
        //    AddInclude("Level4.Payment.BankAccount");
            
        //}
        public LedgerSpecs()
        {
            AddInclude(i => i.Level4);
            AddInclude(i => i.Transactions);
            AddInclude(i => i.Campus);
            AddInclude(i => i.BusinessPartner);
            AddInclude(i => i.Warehouse);
            AddInclude("Level4.Level1");
        }

        public LedgerSpecs(int id) : base(i => i.Id == id
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
        }

        public LedgerSpecs(int id, bool forDoc) : base(i => 
        (forDoc ? i.TransactionId == id : i.Id == id)
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial || i.ReconStatus == DocumentStatus.Reconciled)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
        }

        public LedgerSpecs(bool forDoc, int transactionId) : base(i => i.TransactionId == transactionId
        && (i.ReconStatus == DocumentStatus.Unreconciled)
        && i.IsReconcilable == true)
        {
        }

        public LedgerSpecs(int id, string level4Id, int? businessPartnerId, char sign) 
        : base(i => i.Id == id
        && i.Level4_id == level4Id
        && i.BusinessPartnerId == businessPartnerId
        && i.Sign != sign
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
        }

        public LedgerSpecs(string level4Id, int? businessPartnerId, char sign)
        : base(i => i.Level4_id == level4Id
        && i.BusinessPartnerId == businessPartnerId
        && i.Sign != sign
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
        }
    }
}
