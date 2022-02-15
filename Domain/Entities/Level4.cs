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
        [MaxLength(100)]
        public string Name { get; private set; }
        public Guid Level3_id { get; private set; }
        [ForeignKey("Level3_id")]
        public Level3 Level3 { get; private set; }
        public Guid Level1Id { get; private set; }

        public Level4(Level4 level4)
        {
            Name = level4.Name;
            Level3_id = level4.Level3_id;
            Level1Id = level4.Level1Id;
        }
        protected Level4()
        {
        }

    }
}
