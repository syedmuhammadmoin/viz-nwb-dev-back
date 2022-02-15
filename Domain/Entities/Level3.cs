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
    public class Level3 : BaseEntity<Guid>
    {
        public IEnumerable<Level4> Level4 { get; private set; }
        [MaxLength(100)]
        public string Name { get; private set; }
        public Guid Level2_id { get; private set; }
        [ForeignKey("Level2_id")]
        public Level2 Level2 { get; private set; }
        public Level3(Level3 level3)
        {
            Name = level3.Name;
            Level2_id = level3.Level2_id;
        }
        protected Level3()
        {
        }
    }
}
