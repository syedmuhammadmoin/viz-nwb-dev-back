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
    public class Level1 : BaseEntity<string>, IMustHaveTenant
    {
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }
        public int OrganizationId { get; set; }

        public IEnumerable<Level2> Level2 { get; set; }
        public Level1()
        {
        }
        public Level1(string id, string name, int orgId)
        {
            Id = id;
            Name = name;
            OrganizationId = orgId;
        }
    }
}
