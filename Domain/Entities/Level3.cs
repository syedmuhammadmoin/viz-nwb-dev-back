using Domain.Base;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Level3 : BaseEntity<string>, IMustHaveTenant
    {
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }
        public string Level2_id { get; set; }
        [ForeignKey("Level2_id")]
        public Level2 Level2 { get; private set; }
        public IEnumerable<Level4> Level4 { get; set; }
        public int OrganizationId { get; set; }
        public Level3(string id, string name, string level2_id, int orgId)
        {
            Id = id;
            Name = name;
            Level2_id = level2_id;
            OrganizationId = orgId;
        }
        public Level3()
        {
        }
    }
}
