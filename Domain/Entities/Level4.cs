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

        public Guid Level1_id { get; private set; }
        [ForeignKey("Level1_id")]
        public Level1 Level1 { get; private set; }
        
        protected Level4()
        {
        }

    }
}
