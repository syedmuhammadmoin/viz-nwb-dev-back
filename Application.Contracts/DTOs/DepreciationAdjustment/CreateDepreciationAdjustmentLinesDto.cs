﻿using Domain.Entities;
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
    public class CreateDepreciationAdjustmentLinesDto
    {
        public int? Id { get; set; }
        [Required]
        public int? FixedAssetId { get;  set; }
        [Required]
        public Guid? Level4Id { get;  set; }
        [Required]
        [MaxLength(500)]
        public string Description { get;  set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Debit { get;  set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Credit { get;  set; }
        [Required]
        public bool IsActive { get;  set; }

    }
}
