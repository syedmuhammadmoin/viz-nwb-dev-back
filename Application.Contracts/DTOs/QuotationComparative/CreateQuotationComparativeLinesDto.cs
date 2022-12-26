﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateQuotationComparativeLinesDto
    {
        public int? Id { get; set; }
        [Required]
        public int? QoutationId { get; set; }
    }
}
