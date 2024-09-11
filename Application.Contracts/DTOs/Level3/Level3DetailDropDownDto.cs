using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class Level3DetailDropDownDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Level1Dto Level1 { get; set; }
       
    }
}
