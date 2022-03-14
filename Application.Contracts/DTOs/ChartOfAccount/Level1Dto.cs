﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class Level1Dto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Level2Dto> Level2 { get; set; }
    }
}