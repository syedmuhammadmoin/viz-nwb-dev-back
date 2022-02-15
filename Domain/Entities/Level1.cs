using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Level1 : BaseEntity<Guid>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        public IEnumerable<Level2> Level2 { get; private set; }

        public Level1(Level1 level1)
        {
            Name = level1.Name;
        }
        protected Level1()
        {
        }

    }
}
