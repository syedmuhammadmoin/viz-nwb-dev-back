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
    public class Payment : BaseEntity<int>
    {
        [MaxLength(20)]
        public string DocNo { get; private set; }
        public PaymentType PaymentType { get; private set; } // 0 = Inflow, 1 = Outflow
        public int BusinessPartnerId { get; private set; }
        [ForeignKey("BusinessPartnerId")]
        public BusinessPartner BusinessPartner { get; private set; }
        public DocType PaymentFormType { get; private set; } 
        public Guid AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentRegisterType PaymentRegisterType { get; private set; } // 0 = CashAccount, 1 = BankAccount
        public Guid PaymentRegisterId { get; private set; }
        [ForeignKey("PaymentRegisterId")]
        public Level4 PaymentRegister { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        public int? CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPayment { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SRBTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal IncomeTax { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPayment { get; private set; }
        public int? TransactionId { get; private set; }
        [ForeignKey("TransactionId")]
        public Transactions Transactions { get; private set; }
        public DocumentStatus? BankReconStatus { get; private set; }
        public int? LedgerId { get; private set; }
        public int? DocumentLedgerId { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }

        protected Payment()
        {

        }
        public void setStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void setReconStatus(DocumentStatus status)
        {
            BankReconStatus = status;
        }

        public void setTransactionId(int transactionId)
        {
            TransactionId = transactionId;
        }

        public void setLedgerId(int ledgerId)
        {
            LedgerId = ledgerId;
        }

        public void CreateDocNo()
        {
            if (PaymentType == PaymentType.Inflow)
            {

                //Creating doc no..
                DocNo = "PR-" + String.Format("{0:000}", Id);
            }
            else
            {
                DocNo = "PV-" + String.Format("{0:000}", Id);
            }
        }
    }
}
