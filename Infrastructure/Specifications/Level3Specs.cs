using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class Level3Specs : BaseSpecification<Level3>
    {
        public Level3Specs(string id) : base(i => i.Id == id)
        {
            AddInclude(i => i.Level2);
        }
        public Level3Specs() : base()
        {
            AddInclude(i => i.Level2);
            AddInclude(i => i.Level2.Level1);
        }
    }
}
