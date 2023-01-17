using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class DepreciationDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int UseFullLife { get; set; }
        public Guid AssetAccountId { get; set; }
        public string AssetAccount { get; set; }
        public Guid DeprecationExpenseId { get; set; }
        public string DepreciationExpense { get; set; }
        public Guid AccumulatedDepreciationId { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public string ModelType { get; set; }
        public decimal? DecliningRate { get; set; }
    }
}
