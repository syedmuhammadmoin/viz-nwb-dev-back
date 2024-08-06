using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomUserManager
{
    public class MyRoleValidator : RoleValidator<Role>
    {
        public override async Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
        {
            var roleName = await manager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RoleNameIsNotValid",
                    Description = "Role Name is not valid!"
                });
            }
            else
            {
                var owner = await manager.Roles.FirstOrDefaultAsync(x => x.OrganizationId == role.OrganizationId && x.NormalizedName == roleName);
                if (owner != null && !string.Equals((manager.GetRoleIdAsync(owner)).Result, (manager.GetRoleIdAsync(role)).Result))
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateRoleName",
                        Description = "Role already exist"
                    });
                }
            }
            return IdentityResult.Success;
        }
    }
}
