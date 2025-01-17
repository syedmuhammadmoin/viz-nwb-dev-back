﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateQuotationComparativeDto
    {
        public int? Id { get; set; }
        [Required]
        public DateTime QuotationComparativeDate { get; set; }
        [Required]
        public int? RequisitionId { get; set; }

        [Required]
        public bool? isSubmit { get; set; }
        [Required]
        public List<CreateQuotationComparativeLinesDto> QuotationComparativeLines { get; set; }
    }
}
