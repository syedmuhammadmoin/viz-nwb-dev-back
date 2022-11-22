using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class GeneralLedgerDto
    {
        public int LedgerId { get; set; }
        public int? CampusId { get; set; }
        public Guid Level1Id { get; set; }
        public string Nature { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public int TransactionId { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime DocDate2 { get; set; }
        public DocType? DocType { get; set; }
        public string DocNo { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public int? BId { get; set; }
        public string BusinessPartnerName { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string CampusName { get; set; }
        public bool IsOpeningBalance { get; set; }
    }
}
