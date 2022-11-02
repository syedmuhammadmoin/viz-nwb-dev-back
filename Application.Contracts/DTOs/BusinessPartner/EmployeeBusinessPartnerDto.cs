using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class EmployeeBusinessPartnerDto
    {
        public string Type { get; set; }
        public List<BusinessPartnerDto> BusinessPartner { get; set; }
    }
}
