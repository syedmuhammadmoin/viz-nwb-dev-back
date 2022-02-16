using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Department : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(50)]
        public string Street { get; private set; }
        [MaxLength(50)]
        public string Block { get; private set; }
        [MaxLength(50)]
        public string Road { get; private set; }
        [MaxLength(100)]
        public string HeadOfDept { get; private set; }

        public int OrganizationId { get; private set; }
        [ForeignKey("OrganizationId")]
        public Organization Orgnization { get; private set; }

        public Department(Department department)
        {
            Name = department.Name;
            Country = department.Country;
            State = department.State;
            City = department.City;
            Street = department.Street;
            Block = department.Block;
            Road = department.Road;
            HeadOfDept = department.HeadOfDept;
            OrganizationId = department.OrganizationId;
        }
        protected Department()
        {
        }
    }
}
