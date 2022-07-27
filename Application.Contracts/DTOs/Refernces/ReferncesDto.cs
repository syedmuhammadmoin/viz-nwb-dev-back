using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class ReferncesDto
    {
        public int DocId { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
    }
}
