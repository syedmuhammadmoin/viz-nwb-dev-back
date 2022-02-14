using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(200)]
        public string Address { get; private set; }
        [MaxLength(20)]
        public string Phone { get; private set; }
        [MaxLength(20)]
        public string Fax { get; private set; }
        [MaxLength(20)]
        public string Email { get; private set; }
        [MaxLength(50)]
        public string Website { get; private set; }
        //Industry will link from chart of account in future...
        [MaxLength(50)]
        public string Industry { get; private set; }
        //this will link from tax in future
        [MaxLength(50)]
        public string LegalStatus { get; private set; }
        [MaxLength(50)]
        public string IncomeTaxId { get; private set; }
        [MaxLength(50)]
        public string SalesTaxId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int ClientId { get; private set; }
        public Client Client { get; private set; }

        public Organization(Organization organization)
        {
            Name = organization.Name;
            Description = organization.Description;
            Country = organization.Country;
            State = organization.State;
            City = organization.City;
            Address = organization.Address;
            Phone = organization.Phone;
            Fax = organization.Fax;
            Email = organization.Email;
            Website = organization.Website;
            Industry = organization.Industry;
            LegalStatus = organization.LegalStatus;
            IncomeTaxId = organization.IncomeTaxId;
            SalesTaxId = organization.SalesTaxId;
            StartDate = organization.StartDate;
            EndDate = organization.EndDate;
            ClientId = organization.ClientId;
        }

        protected Organization()
        {

        }
    }
}
