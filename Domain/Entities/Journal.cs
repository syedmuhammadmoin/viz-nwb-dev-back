using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Journal : BaseEntity<int>
    {
        public string Name { get; private set; }
        public Types Type { get; private set; }
      
    }
}
