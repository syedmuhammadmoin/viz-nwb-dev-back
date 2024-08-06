using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class UpdateOrganizationDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; private set; }
        public string Address { get; set; }
        public string Phone { get; private set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        //Industry will link from chart of account in future...
        public string Industry { get; set; }
        //this will link from tax in future
        public string LegalStatus { get; set; }
        public string IncomeTaxId { get; set; }
        public string GSTRegistrationNo { get; set; }
        public DateTime? FiscalYearStart { get; set; }
        public DateTime? FiscalYearEnd { get; set; }
        public string Currency { get; set; }
    }
}
