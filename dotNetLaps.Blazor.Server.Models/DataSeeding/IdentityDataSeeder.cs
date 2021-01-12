using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Models
{
    public class IdentityDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityDataSeeder(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            var superAdminRole = new IdentityRole
            {
                Id = "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            };
            await CreateRoleAsync(superAdminRole);

            var superAdminUserPassword = "P@ssword1";
            var superAdminUser = new ApplicationUser
            {
                Id = "b8633e2d-a33b-45e6-8329-1958b3252bbd",
                UserName = "admin@example.nl",
                NormalizedUserName = "ADMIN@EXAMPLE.NL",
                Email = "admin@example.nl",
                NormalizedEmail = "ADMIN@EXAMPLE.NL",
                EmailConfirmed = true,
            };
            await CreateUserAsync(superAdminUser, superAdminUserPassword);

            var superAdminInRole = await _userManager.IsInRoleAsync(superAdminUser, superAdminRole.Name);
            if (!superAdminInRole)
                await _userManager.AddToRoleAsync(superAdminUser, superAdminRole.Name);
        }

        private async Task CreateRoleAsync(IdentityRole role)
        {
            var exits = await _roleManager.RoleExistsAsync(role.Name);
            if (!exits)
                await _roleManager.CreateAsync(role);
        }

        private async Task CreateUserAsync(ApplicationUser user, string password)
        {
            var exists = await _userManager.FindByEmailAsync(user.Email);
            if (exists == null)
                await _userManager.CreateAsync(user, password);
        }
    }
}
