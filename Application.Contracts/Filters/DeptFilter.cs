﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class DeptFilter
    {
        public int?[] DepartmentId { get; set; }
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public int? CampusId { get; set; }
        public string AccountPayableId { get; set; }
    }
}
