﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CampusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string SalesTaxId { get; set; }
        public string NTN { get; set; }
        public string SRB { get; set; }
    }
}
