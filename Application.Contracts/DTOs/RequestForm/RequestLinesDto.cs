using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class RequestLinesDto
    {
        public int Id { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
    }
}
