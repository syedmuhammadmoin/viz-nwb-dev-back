using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Filters
{
    public class JournalFilters
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Types Types { get; set; }
    }
}
