using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class JournalDto
    {
        public int Id { get; set; }

        public string Name { get;  set; }

        public Types Type { get;  set; }
    }
}
