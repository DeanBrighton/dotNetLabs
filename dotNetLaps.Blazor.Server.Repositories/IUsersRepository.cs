using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dotNetLabs.Blazor.Server.Models;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public interface IUsersRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<ApplicationUser> GetUserByEmailAsync(string email);

        Task CreateuserAsync(ApplicationUser user, string password, string role);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<string> GetUserRoleAsync(ApplicationUser user);


    }
}
