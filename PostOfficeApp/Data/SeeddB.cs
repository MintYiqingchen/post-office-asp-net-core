using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PostOfficeApp.Models;

namespace PostOfficeApp.Data
{
    public class SeedDB
    {
        public static async Task Initialize(IServiceProvider serviceProvider,DbContext context, string pw)
        {
            // var adminID = await EnsureUser(serviceProvider, pw, "821244608@qq.com");
            // await EnsureRole(serviceProvider, adminID, Constants.AdministratorRole);

            //var uid = await EnsureUser(serviceProvider, pw, "manager@postoffice.com");
            //await EnsureRole(serviceProvider, uid, Constants.ManageRole);
            var uid = await EnsureUser(serviceProvider, "client", "client@postoffice.com");
            await EnsureRole(serviceProvider, uid, Constants.ClientRole);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string pw, string UserName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName };
                await userManager.CreateAsync(user, pw);
            }
            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            IdentityResult ir = null;
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!await roleManager.RoleExistsAsync(role))
            {
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(uid);
            // admin can not be a client
            if (await userManager.IsInRoleAsync(user,Constants.ClientRole) 
                && role.Equals(Constants.AdministratorRole))
            {
                await userManager.RemoveFromRoleAsync(user, Constants.ClientRole);
            }
            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}
