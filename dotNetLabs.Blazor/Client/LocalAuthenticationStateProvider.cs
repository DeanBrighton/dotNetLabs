using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Client
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storageService;

        public LocalAuthenticationStateProvider(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            try
            {
                if (await _storageService.ContainKeyAsync("access_token"))
                {
                    //Decrypt the token to use the content
                    string accessToken = await _storageService.GetItemAsStringAsync("access_token");
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(accessToken);
                    var identity = new ClaimsIdentity(jwt.Claims, "Bearer");

                    var user = new ClaimsPrincipal(identity);
                    var state = new AuthenticationState(user);

                    //Notify the application of the state change
                    NotifyAuthenticationStateChanged(Task.FromResult(state));

                    //return the user with all the claims.
                    return state;

                }
                else
                    //return an empty claims principal
                    return new AuthenticationState(new System.Security.Claims.ClaimsPrincipal());

            }
            catch(Exception e)
            {

                Console.WriteLine(e.Message);
                //Return an empty claims principal
                return new AuthenticationState(new System.Security.Claims.ClaimsPrincipal());
            }



        }

        public async Task LogOutAsync()
        {

            await _storageService.RemoveItemAsync("access_token");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));


        }
    }
}
