using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateJournalDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Journal Name")]
        public string Name { get;  set; }

        [Required(ErrorMessage = "Journal Name")]
        public Types Type { get;  set; }
    }
}
