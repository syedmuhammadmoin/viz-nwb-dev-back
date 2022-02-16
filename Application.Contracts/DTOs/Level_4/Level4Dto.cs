using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class Level4Dto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Level3_id { get; set; }
        public string levle3Name { get; set; }
    }
}
