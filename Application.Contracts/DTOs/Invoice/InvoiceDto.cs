using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Contact { get; set; }
        public DocumentStatus Status { get; set; }
        public decimal TotalBeforeTax { get; private set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual List<InvoiceLinesDto> InvoiceLines { get; set; }
    }
}
