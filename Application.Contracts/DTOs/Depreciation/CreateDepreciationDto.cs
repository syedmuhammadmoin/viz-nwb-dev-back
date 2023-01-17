using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateDepreciationDto
    {
        public int Id { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public int UseFullLife { get; set; }
        [Required]
        public Guid AssetAccountId { get; set; }
        [Required]
        public Guid DepricationExpenseId { get; set; }
        [Required]
        public Guid AccumulatedDepriciationId { get; set; }
        [Required]
        public DepreciationMethod ModelType { get; set; }
        [Required]
        [Range(0.00, 100.00, ErrorMessage = "Please enter a value between 0 and 100")]
        public decimal? DecliningRate { get; set; }
    }
}
