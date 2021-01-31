using dotNetLabs.Blazor.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Services
{
    public interface IUsersService
    {
        //Task RegisterUserAsync();
        Task<LoginResponse> GenerateTokenAsync(LoginRequest model);

        Task<OperationResponse<string>> RegisterUserAsync(RegisterRequest model);

    }

}
