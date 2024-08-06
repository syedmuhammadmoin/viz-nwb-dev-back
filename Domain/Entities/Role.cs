using Domain.Base;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : IdentityRole, IMustHaveTenant
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        public bool IsDelete { get; set; }
        [MaxLength(100)]
        public int OrganizationId { get; set; }

        public Role()
        {

        }

        public Role(string roleName, int orgId)
        {
            Name = roleName;
            OrganizationId = orgId;
        }

    }
  
}
