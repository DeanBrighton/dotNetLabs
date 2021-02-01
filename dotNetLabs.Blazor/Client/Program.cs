using Blazored.LocalStorage;
using BlazorFluentUI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //Init the HTTP client
            //Added custom HTTP client from Microsoft.Extensions.Http... Note sure why though.
            // to have an different HTTP handler for each API the client may call.
            builder.Services.AddHttpClient("dotNetLabs.Api",client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

            }).AddHttpMessageHandler<AuthorizationMessageHandler>();
            // Used to creat new instances of the HTTP client by name
            builder.Services.AddTransient(sp => sp.GetService<IHttpClientFactory>().CreateClient("dotNetLabs.Api"));
            
            
            //Registeres the MessageHandler "Middleware" used to attach any available access token to the header
            builder.Services.AddTransient<AuthorizationMessageHandler>();

            //Register the local authentication state provider
            builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();


            //Add blazor local storage service.
            builder.Services.AddBlazoredLocalStorage();



            builder.Services.AddBlazorFluentUI();
            builder.Services.AddAuthorizationCore();



            await builder.Build().RunAsync();
        }
    }
}
