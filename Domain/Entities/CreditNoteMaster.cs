﻿using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CreditNoteMaster : BaseEntity<int>
    {
        public int CustomerId { get; private set; }
        [ForeignKey("CustomerId")]
        public BusinessPartner Customer { get; private set; }
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public DateTime NoteDate { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBeforeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; private set; }
        public DocumentStatus Status { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public virtual List<CreditNoteLines> CreditNoteLines { get; private set; }
        protected CreditNoteMaster()
        {
        }

        public void setStatus(DocumentStatus status)
        {
            Status = status;
        }
        public void setTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }
        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "CRN-" + String.Format("{0:000}", Id);
        }
    }
}
