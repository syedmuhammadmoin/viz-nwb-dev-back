using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager, int orgId)
        {
            await roleManager.CreateAsync(new Role(Roles.SuperAdmin.ToString(), orgId));
            await roleManager.CreateAsync(new Role(Roles.Admin.ToString(), orgId));
            await roleManager.CreateAsync(new Role(Roles.Applicant.ToString(), orgId));
        }
        public static async Task<Role> SeedAsync(RoleManager<Role> roleManager, int orgId)
        {
            var role = new Role()
            {
                Name = Roles.SuperAdmin.ToString(),
                OrganizationId = orgId,
            };
            await roleManager.CreateAsync(role);
            return role;
        }
    }
}
