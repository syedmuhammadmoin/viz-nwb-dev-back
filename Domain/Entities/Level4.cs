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
    public class Level4 : BaseEntity<Guid>
    {
        [MaxLength(200)]
        public string Name { get; set; }
        public Guid Level3_id { get; set; }
        [ForeignKey("Level3_id")]
        public Level3 Level3 { get; private set; }
        public Guid Level1_id { get; set; }
        [ForeignKey("Level1_id")]
        public Level1 Level1 { get; private set; }
        
        public Level4()
        {
        }

        public Level4(string name, Guid level3_id, Guid level1_id)
        {
            Name = name;
            Level3_id = level3_id;
            Level1_id = level1_id;
        }
    }
}
