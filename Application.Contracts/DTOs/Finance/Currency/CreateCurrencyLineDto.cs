using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCurrencyLineDto
    {
        public int? Id { get; set; }
        
        public int? CurrencyId { get; set; }
        [Required(ErrorMessage ="Date is required")]
        public DateTime Date { get; set; }
        //Fix: UnitPerUSD and USDPerUnit inverse proportionality this should be validate on server .
        [Required(ErrorMessage ="Unit Per USD is required")]
        [Range(0.000001,double.MaxValue, ErrorMessage ="Unit Per USD must be greater than 0")]
        public decimal UnitPerUSD { get; set; }
        // UnitPerUSD
        [Required]
        [Range(0.000001,double.MaxValue,ErrorMessage ="USD Per Unit must be greater than 0")]
        public decimal USDPerUnit { get; set; }
    }
}
