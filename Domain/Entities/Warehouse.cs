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
    public class Warehouse : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(50)]
        public string StoreManager { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(200)]
        public string Address { get; private set; }
        [MaxLength(50)]
        public string Manager { get; private set; }
        public int DepartmentId { get; private set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; private set; }
        public virtual List<Location> Locations { get; private set; }
        public int OrganizationId { get; set; }

        protected Warehouse()
        {
        }
    }
}
