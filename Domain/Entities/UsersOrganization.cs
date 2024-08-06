using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsersOrganization : BaseEntity<int>
    {
        public string UserId { get; private set; }
        [ForeignKey("UserId")]
        public User User { get; private set; }
        public int OrgId { get; private set; }
        [ForeignKey("OrgId")]
        public Organization Organization { get; private set; }

        protected UsersOrganization()
        {

        }

        public UsersOrganization(string userId, int orgId)
        {
            UserId = userId;
            OrgId = orgId;
        }
    }
}
