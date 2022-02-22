using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BillDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Contact { get; set; }
        public DocumentStatus Status { get; set; }
        public decimal TotalBeforeTax { get; private set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionId { get; set; }
        public virtual List<BillLinesDto> BillLines { get; set; }
    }
}
