using Domain.Base;
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
    public class BankStmtLines : BaseEntity<int>
    {
        public int Reference { get; private set; }
        public DateTime StmtDate { get; private set; }
        [MaxLength(50)]
        public string Label { get; private set; }
        public DocumentStatus BankReconStatus { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public BankStmtMaster BankStmtMaster { get; private set; }

        protected BankStmtLines()
        {

        }

        public BankStmtLines(int reference, DateTime stmtDate, string label, DocumentStatus bankReconStatus, decimal debit, decimal credit)
        {
            Reference = reference;
            StmtDate = stmtDate;
            Label = label;
            BankReconStatus = bankReconStatus;
            Debit = debit;
            Credit = credit;
        }
        public void UpdateStatus(DocumentStatus status)
        {
            BankReconStatus = status;
        }
    }
}
