﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class Level3Dto
    {
        public string Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public IEnumerable<Level4Dto> children { get; set; }
    }
}
