using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Data
{
    public static class SeedData
    {
        public async static Task seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
           await SeedRoles(roleManager);
           await SeedUsers(userManager);
        }
        public async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("customer@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "customer1",
                    Email = "customer@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "P@asswrd1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            if (await userManager.FindByEmailAsync("admin@Bookstore.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@Bookstore.com"
                };
                var result = await userManager.CreateAsync(user, "P@asswrd1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }
        public async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                 await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
