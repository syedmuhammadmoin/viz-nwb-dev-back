using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Tax
{
    public class ChildrenTaxDto
    {
        public int TaxId { get; set; }
        public string Name { get; set; }
        public TaxComputation? TaxComputation { get; set; }
        public decimal Amount { get; set; }
    }
}
