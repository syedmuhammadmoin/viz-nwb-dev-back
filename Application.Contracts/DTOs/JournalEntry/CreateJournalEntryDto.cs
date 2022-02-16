﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class CreateJournalEntryDto
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public virtual IEnumerable<CreateJournalEntryLinesDto> JournalEntryLines { get; set; }
    }
}
