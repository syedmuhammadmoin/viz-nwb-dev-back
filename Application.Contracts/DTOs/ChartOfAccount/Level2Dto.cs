using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class Level2Dto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Level3Dto> Level3 { get; set; }
    }
}
