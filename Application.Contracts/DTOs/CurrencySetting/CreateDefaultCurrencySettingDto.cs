using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.CurrencySetting
{
    public class CreateDefaultCurrencySettingDto
    {
        public int Id { get; set; }
        public int? CurrencyId { get; set; }
        public int? OrganizationId { get; set; }
    }
}
