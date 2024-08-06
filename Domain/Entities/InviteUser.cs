using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InviteUser : BaseEntity<int>
    {
        public string Email { get; private set; }
        public int OrgId { get; private set; }
        [ForeignKey("OrgId")]
        public Organization Organization { get; set; }
        public string RoleId { get; private set; }
        [ForeignKey("RoleId")]
        public Role Role { get; private set; }
        public bool isAccepted { get; private set; }
        
        

        protected InviteUser()
        {

        }

        public InviteUser(string email, int orgId, string roleId)
        {
            Email = email;
            OrgId = orgId;
            RoleId = roleId;
            isAccepted = false;
        }
        public void accepted()
        {
            isAccepted  = true;
        }
    }
}
