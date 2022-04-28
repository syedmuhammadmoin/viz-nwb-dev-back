﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class DesignationDto
    {
        public int? Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
