using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class InviteUserSpecs : BaseSpecification<InviteUser>
    {
        public InviteUserSpecs(int organizationId) : base(e => e.OrgId == organizationId && e.IsDelete == false)
        {
        }
        public InviteUserSpecs(string email, bool isAccepted) : base(e => e.Email == email && e.isAccepted == isAccepted && e.IsDelete == false)
        {
        }
        public InviteUserSpecs(string email, int organizationId) : base(e => e.Email == email && e.OrgId == organizationId && e.IsDelete == false)
        {
        }
    }

}
