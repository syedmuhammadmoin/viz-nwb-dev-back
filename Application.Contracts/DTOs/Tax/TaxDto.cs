using Application.Contracts.DTOs.Tax;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class TaxDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TaxType TaxType { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public TaxComputation TaxComputation { get; set; }
        public string Description { get; set; }
        public string LegalNotes { get; set; }
        public decimal Amount { get; set; }
        public TaxScope TaxScope { get; set; }
        public virtual List<TaxInvoiceLinesDto> TaxInvoicesLines { get; set; }        
        public virtual List<TaxRefundLinesDto> TaxRefundLines { get; set; }
    }
}
