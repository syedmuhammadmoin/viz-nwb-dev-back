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
        public string Name { get; private set; }
        public Guid Level3_id { get; private set; }
        [ForeignKey("Level3_id")]
        public Level3 Level3 { get; private set; }
        [ForeignKey("Level1_id")]
        public Guid Level1_Id { get; private set; }
        public Level1 Level1 { get; private set; }
        public Level4(Level4 level4)
        {
            Name = level4.Name;
            Level3_id = level4.Level3_id;
            Level1_Id = level4.Level1_Id;
        }
        protected Level4()
        {
        }

    }
}
