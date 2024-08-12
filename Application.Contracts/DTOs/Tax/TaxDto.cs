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
    }
}
