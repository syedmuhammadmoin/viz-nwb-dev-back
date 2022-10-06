using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public class UserSeeding
    {
        public static async void Initialize(IServiceProvider services, string password)
        {
            using (var scope = services.CreateScope())
            {
                var ser = scope.ServiceProvider;
                var userManager = ser.GetRequiredService<UserManager<User>>();
                var roleManager = ser.GetRequiredService<RoleManager<IdentityRole>>();
                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultUsers.SeedSuperAdminAsync(userManager, roleManager, password);
            }
        }
    }
}
