using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Tax
{
    public class CreateTaxInvoiceLinesDto
    {
        public int Id { get; set; }
        public TaxBase? TaxBase { get; set; }
        public string? AccountId { get; set; }     
    }
}
