using Application.Contracts.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Helper
{
    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RegisterRoleClaimsDto> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RegisterRoleClaimsDto { Value = fi.GetValue(null).ToString(), Type = "Permission" });
            }
        }
        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }

    }
}
