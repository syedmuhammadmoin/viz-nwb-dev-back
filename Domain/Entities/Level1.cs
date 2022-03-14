using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Level1 : BaseEntity<Guid>
    {
        [MaxLength(200)]
        public string Name { get; set; }
        public IEnumerable<Level2> Level2 { get; set; }
        public Level1(Level1 level1)
        {
            Name = level1.Name;
        }
        public Level1()
        {
        }
    }
}
