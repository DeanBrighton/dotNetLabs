﻿using System.Linq;
using System.Threading.Tasks;
using dotNetLabs.Blazor.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public class IdentityUsersRepository : IUsersRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public IdentityUsersRepository(UserManager<ApplicationUser> userManager,
                                       RoleManager<IdentityRole> roleManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task CreateuserAsync(ApplicationUser user, string password, string role)
        {
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);

        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
           
            var roles =  await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }
    }
}
