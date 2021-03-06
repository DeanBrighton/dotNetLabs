﻿using dotNetLabs.Blazor.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.DataSeeding
{
    public class UsersSeeding
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersSeeding(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task SeedData()
        {

            //Check and create roles
            try
            {
               

                if (await _roleManager.FindByNameAsync("Admin") != null)
                    return;


                var adminRole = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(adminRole);

                var userRole = new IdentityRole { Name = "User" };
                await _roleManager.CreateAsync(userRole);


                // Create users
                var admin = new ApplicationUser
                {
                    Email = "admin@brightonmail.net",
                    UserName = "admin@brightonmail.net",
                    FirstName = "Admin",
                    LastName = "User"
                };

                await _userManager.CreateAsync(admin, "P@ssw0rd123");
                await _userManager.AddToRoleAsync(admin, "Admin");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }


    }
}
