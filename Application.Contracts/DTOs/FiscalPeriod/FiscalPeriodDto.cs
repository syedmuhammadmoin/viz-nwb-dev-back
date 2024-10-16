using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.FiscalPeriod
{
    public class FiscalPeriodDto
    {
        public int Id { get; set; }  
        public string Name { get; set; }    
        public DateTime? StartDate { get; set; }     
        public DateTime? EndDate { get; set; }
        public int? OrganizationId { get; set; }
    }
}
