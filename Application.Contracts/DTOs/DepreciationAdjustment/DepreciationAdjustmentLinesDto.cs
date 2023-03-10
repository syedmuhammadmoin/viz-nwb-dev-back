using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class DepreciationAdjustmentLinesDto
    {
        public int Id { get; set; }
        public int FixedAssetId { get; set; }
        public string FixedAsset { get; set; }
        public Guid Level4Id { get; set;}
        public string Level4 { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool IsActive { get; set; }
        public int MasterId { get; set; }
    }
}
