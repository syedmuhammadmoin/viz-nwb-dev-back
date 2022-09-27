using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Level2 : BaseEntity<Guid>
    {
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }
        public IEnumerable<Level3> Level3 { get; set; }
        public Guid Level1_id { get; set; }
        [ForeignKey("Level1_id")]
        public Level1 Level1 { get; private set; }
        public Level2()
        {
        }
    }
}
