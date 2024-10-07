using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateCurrencyDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        [MaxLength(3, ErrorMessage = "Code cannot exceed 3 characters.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Symbol is required.")]
        [MaxLength(5, ErrorMessage = "Symbol cannot exceed 5 characters.")]
        public string Symbol { get; set; }
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "Unit cannot exceed 50 characters.")]
        public string Unit { get; set; }
        [MaxLength(50, ErrorMessage = "SubUnit cannot exceed 50 characters.")]
        public string SubUnit { get; set; }
        public IEnumerable<CreateCurrencyLineDto> CurrencyLines { get; set; }
    }
}
