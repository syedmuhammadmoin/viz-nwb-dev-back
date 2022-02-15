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
        public string Name { get; private set; }
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(20)]
        public string Street { get; private set; }
        [MaxLength(20)]
        public string Block { get; private set; }
        [MaxLength(20)]
        public string Road { get; private set; }
        [MaxLength(50)]
        public string Manager { get; private set; }
        public int DepartmentId { get; private set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; private set; }
        public virtual List<Location> Locations { get; private set; }

        public Warehouse(Warehouse warehouse)
        {
            Name = warehouse.Name;
            Country = warehouse.Country;
            State = warehouse.State;
            City = warehouse.City;
            Street = warehouse.Street;
            Block = warehouse.Block;
            Road = warehouse.Road;
            Manager = warehouse.Manager;
            DepartmentId = warehouse.DepartmentId;
            Locations = warehouse.Locations;
        }
        protected Warehouse()
        {

        }
    }
}
