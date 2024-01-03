﻿using Domain.Constants;
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
        public LedgerSpecs(string joingString, Guid AccountID) : base(i => i.Level4_id== AccountID)
        {
            switch (joingString)
            {
                case "Accounts":
                    AddInclude(i => i.Level4);
                    AddInclude("Level4.Level1");
                    break; 
                default:
                    break;
            }
            
        }
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

        public LedgerSpecs(int id, Guid level4Id, int? businessPartnerId, char sign) 
        : base(i => i.Id == id
        && i.Level4_id == level4Id
        && i.BusinessPartnerId == businessPartnerId
        && i.Sign != sign
        && (i.ReconStatus == DocumentStatus.Unreconciled || i.ReconStatus == DocumentStatus.Partial)
        && i.IsReconcilable == true)
        {
            AddInclude(i => i.Transactions);
        }

        public LedgerSpecs(Guid level4Id, int? businessPartnerId, char sign)
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
