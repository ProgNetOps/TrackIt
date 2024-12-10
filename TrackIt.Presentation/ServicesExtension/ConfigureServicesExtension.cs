using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrackIt.Domain;
using TrackIt.Persistence;
using TrackIt.Presentation.Mappings;
using TrackIt.Presentation.Utilities;
using TrackIt.Repository.Implementations;
using TrackIt.Repository.Services;

namespace TrackIt.Presentation.ServicesExtension
{
    /// <summary>
    /// A static class for extension method where services are added to the services container, to declutter the program class
    /// </summary>
    public static class ConfigureServicesExtension
    {
        /// <summary>
		/// Extension method where the services are added to declutter the program.cs file
		/// </summary>
		/// <param name="services"></param>
        public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            //* Add services to the container*//

            // Add MVC Service
            // Apply Authorize Attribute Globally
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder().
                RequireAuthenticatedUser().
                Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddRazorRuntimeCompilation();//AddRazorRuntimeCompilation() fixes the -view not found- bug

            //Add Db Context to the container
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            
            //Add BTS Service
            services.AddScoped<IBaseStationService, BaseStationService>();

            //Add IPPoP Service
            services.AddScoped<IIPPoPService, IPPoPService>();

            //Add DCNRouter Service
            services.AddScoped<IDCNRouterService, DCNRouterService>();

            //Add Switch Service
            services.AddScoped<INetworkSwitchService, NetworkSwitchService>();

            //Add Client Service
            services.AddScoped<IClientService, ClientService>();

            //Add Circuit Service
            services.AddScoped<ICircuitService, CircuitService>();

            //Add Account Service
            services.AddScoped<IAccountService, AccountService>();

            //Add Administration Service
            services.AddScoped<IAdminService, AdminService>();
                        
            //Add Ticket Service
            services.AddScoped<ITicketService, TicketService>();

            //Add Automapper 
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            //Add Identity and configure identity options
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            }).
            AddEntityFrameworkStores<AppDbContext>();

            //Add Claims Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));

                //options.AddPolicy("AdminPolicy",
                //    policy => policy.RequireClaim(ClaimTypes.Gender, "Female").
                //    RequireClaim();

            });

            //Add service for getting values of the properties of logged in identity user
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationuserClaimsPrincipalFactory>();

            return services;
        }
    }
}
