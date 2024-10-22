using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateUnitOfMeasurementDto
    {
        public int? Id { get; set; }       
        public bool? IsKilogram { get; set; }
        public bool? IsPound { get; set; }
        public bool? IsCubicMeterVol { get; set; }
        public bool? IsCubicFeetVol { get; set; }
        public int OrganizationId { get; set; }

    }
}
