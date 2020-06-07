using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Constants;
namespace Microsoft.WebApplication1.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.ADMINISTRATORS));
            string regUsername = "shehanatuk@gmail.com";
            string regEmail = "shehanatuk@gmail.com";
            var defaultUser = new ApplicationUser { UserName = regUsername, Email = regEmail };
            await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD); //save user object with default password

            string adminUsername = "admin@gmail.com";
            string adminEmail = "admin@gmail.com";
            var adminUser = new ApplicationUser { UserName = adminUsername, Email = adminEmail };
            await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(adminUsername); // find admin by username and save instance to adminUser object
            await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.Roles.ADMINISTRATORS); // giving the admin user admin role 

        }
    }
}
