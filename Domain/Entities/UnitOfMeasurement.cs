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
    public class UnitOfMeasurement : BaseEntity<int>
    {
        [MaxLength(50)]
        public string Name { get; private set; }
        public bool? IsKilogram { get; private set; }
        public bool? IsPound { get; private set; }
        public bool? IsCubicMeterVol { get; private set; }
        public bool? IsCubicFeetVol { get; private set; }
        public int OrganizationId { get; private set; }
        [ForeignKey("OrganizationId")]
        public Organization Organization { get; private set; }


        protected UnitOfMeasurement()
        {

        }
    }
}
