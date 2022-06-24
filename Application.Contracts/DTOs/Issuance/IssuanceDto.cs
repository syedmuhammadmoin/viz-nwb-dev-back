﻿using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class IssuanceDto
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime IssuanceDate { get; set; }
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public DocumentStatus State { get; set; }
        public virtual List<IssuanceLinesDto> IssuanceLines { get; set; }
        public bool IsAllowedRole { get; set; }
    }
}
