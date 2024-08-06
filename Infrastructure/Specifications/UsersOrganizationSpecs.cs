using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class UsersOrganizationSpecs : BaseSpecification<UsersOrganization>
    {
        public UsersOrganizationSpecs(string userId) : base(e => e.UserId == userId)
        {
            AddInclude(i => i.User);
            AddInclude(i => i.Organization);
        }

        public UsersOrganizationSpecs(string userId, int organizationId) : base(e => e.UserId == userId && e.OrgId == organizationId)
        {
        }

        public UsersOrganizationSpecs(string userId, int? organizationId) : base(e => e.UserId == userId && e.OrgId == organizationId)
        {
            AddInclude(i => i.User);
            AddInclude(i => i.Organization);
        }

        public UsersOrganizationSpecs(string currentUserId, int? organizationId, bool forInviteUsers) : base(e => e.UserId != currentUserId && e.OrgId == organizationId)
        {
            AddInclude(i => i.User);
        }
    }
}
