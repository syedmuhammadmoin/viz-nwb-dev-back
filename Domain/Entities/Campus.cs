﻿using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Campus : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        public int OrganizationId { get; private set; }
        [ForeignKey("OrganizationId")]
        public Organization Organization { get; private set; }

        protected Campus()
        {
        }
    }
}