using AutoMapper;
using dotNetLabs.Blazor.Server.Infrastructure;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Server.Profiles;
using dotNetLabs.Blazor.Server.Repositories;
using dotNetLabs.Blazor.Server.Services;
using dotNetLabs.Blazor.Server.Services.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),sqlOptions => {
                    sqlOptions.MigrationsAssembly("dotNetLabs.Blazor.Server");
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(3),
                        errorNumbersToAdd: null);
                });

            });
            

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;


            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
              {
                  auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

              }).AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidAudience = Configuration["AuthSettings:Audience"],
                      ValidIssuer = Configuration["AuthSettings:Issuer"],
                      RequireExpirationTime = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
                      ValidateIssuerSigningKey = true
                  };

              });

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            //TODO: Using attributes to register services.
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();


            services.AddScoped(sp => new AuthOptions
            {
                Audience = Configuration["AuthSettings:Issuer"],
                Issuer = Configuration["AuthSettings:Issuer"],
                Key = Configuration["AuthSettings:Key"]

            });

            services.AddHttpContextAccessor();
            services.AddScoped(sp =>
            {
                var httpContext = sp.GetService<IHttpContextAccessor>().HttpContext;
                var identityOptions = new Infrastructure.IdentityOptions();

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    string id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    string firstname = httpContext.User.FindFirst(ClaimTypes.GivenName).Value;
                    string lastname = httpContext.User.FindFirst(ClaimTypes.Surname).Value;
                    string email = httpContext.User.FindFirst(ClaimTypes.Email).Value;
                    string role = httpContext.User.FindFirst(ClaimTypes.Role).Value;

                    identityOptions.UserID = id;
                    identityOptions.FullName = $"{firstname} {lastname}";
                    identityOptions.Email = email;
                    identityOptions.IsAdmin = role == "Admin" ? true : false;

                }

                return identityOptions;
            });

            services.AddSingleton(sp =>
            {
                return new EnvironmentOptions()
                {
                    ApiUrl = Configuration["ApiURL"]
                };
                                
            });


            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                config.AddProfile(new VideoProfile(provider.GetService<EnvironmentOptions>()));

            }).CreateMapper());

            services.AddSwaggerGen();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            

             var dataSeeding = new DataSeeding.UsersSeeding(userManager, roleManager);
            dataSeeding.SeedData().Wait();


            //var IdentitySeeding = new IdentityDataSeeder(userManager, roleManager);
            //IdentitySeeding.SeedAsync();




            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "dotNet Labs API");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });




        }
    }
}
