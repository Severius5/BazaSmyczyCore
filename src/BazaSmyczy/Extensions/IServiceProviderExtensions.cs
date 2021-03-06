﻿using BazaSmyczy.Core.Configs;
using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Stores.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BazaSmyczy.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static async Task CreateRoles(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in Roles.AllRoles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task CreateAdminUser(this IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminConfig = configuration.GetSection("AdminAccountConfig").Get<AdminAccountConfig>();
            var user = new ApplicationUser { UserName = adminConfig.AdminUsername, Email = adminConfig.AdminEmail, EmailConfirmed = true };

            if (await userManager.FindByNameAsync(adminConfig.AdminUsername) == null)
            {
                var result = await userManager.CreateAsync(user, adminConfig.AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Administrator);
                }
            }
        }

        public static void UpdateDb(this IServiceProvider serviceProvider)
        {
            var leashContext = serviceProvider.GetRequiredService<LeashDbContext>();
            var appContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            leashContext.Database.Migrate();
            appContext.Database.Migrate();
        }
    }
}
