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
        public Guid DepricationExpenseId { get; set; }
        public string DepricationExpense { get; set; }
        public Guid AccumulatedDepriciationId { get; set; }
        public string AccumulatedDepriciation { get; set; }
        public string ModelType { get; set; }
        public decimal? DecliningRate { get; set; }
    }
}
