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
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string Phone { get; private set; }
        [MaxLength(20)]
        public string Fax { get; private set; }
        [MaxLength(20)]
        public string Email { get; private set; }
        [MaxLength(100)]
        public string Website { get; private set; }
        //Industry will link from chart of account in future...
        [MaxLength(100)]
        public string Industry { get; private set; }
        //this will link from tax in future
        [MaxLength(100)]
        public string LegalStatus { get; private set; }
        [MaxLength(100)]
        public string IncomeTaxId { get; private set; }
        [MaxLength(100)]
        public string GSTRegistrationNo{ get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime FiscalYear { get; private set; }

        protected Organization()
        {

        }
    }
}
