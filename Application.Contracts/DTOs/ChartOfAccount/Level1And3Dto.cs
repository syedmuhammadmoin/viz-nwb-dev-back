using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class Level1And3Dto
    {
        public string Id { get; set; }
        [MaxLength(200)]
        public string Level1Name { get; set; }
        public IEnumerable<Level3Dto> children { get; set; }
    }
}
