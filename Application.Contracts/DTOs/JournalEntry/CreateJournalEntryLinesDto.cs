﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateJournalEntryLinesDto
    {
        public int Id { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        public int? BusinessPartnerId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal Debit { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive value")]
        public decimal Credit { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
}