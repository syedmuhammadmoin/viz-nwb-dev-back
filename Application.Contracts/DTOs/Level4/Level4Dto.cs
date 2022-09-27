using Domain.Constants;
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
        public string EditableName { get; set; }
        public AccountType AccountType { get; set; }
        public Guid Level3_id { get; set; }
        public string Level3Name { get; set; }
    }
}
