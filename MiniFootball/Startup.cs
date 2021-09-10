namespace MiniFootball
{
    using AspNetCoreHero.ToastNotification;
    using Data;
    using Data.Models;
    using Hubs;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services.Admins;
    using Services.Cities;
    using Services.Countries;
    using Services.Fields;
    using Services.Games;
    using Services.Statistics;
    using Services.Users;
    using System.Security.Claims;

    using static GlobalConstant;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<MiniFootballDbContext>(options =>
                    options
                        .UseSqlServer(configuration
                            .GetConnectionString(DefaultConnection)));
            services
                .AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MiniFootballDbContext>();

            services
                .AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>()
                .AddEntityFrameworkStores<MiniFootballDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services
                .AddAutoMapper(typeof(Startup));

            services
                .AddSignalR();

            services
                .AddMemoryCache();

            services
                .AddControllersWithViews(options =>
                    options
                        .Filters
                        .Add<AutoValidateAntiforgeryTokenAttribute>());

            services
                .AddTransient<ClaimsPrincipal>(options => 
                    options
                        .GetService<IHttpContextAccessor>()?
                        .HttpContext?
                        .User);

            services
                .AddNotyf(config =>
                {
                    config.DurationInSeconds = Notifications.DefaultDurationInSeconds;
                    config.IsDismissable = true;
                    config.Position = NotyfPosition.TopCenter;
                });

            services
                .AddTransient<ICityService, CityService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IGameService, GameService>()
                .AddTransient<IFieldService, FieldService>()
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<IStatisticsService, StatisticsService>()
                .AddTransient<ICountryService, CountryService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseMigrationsEndPoint();
            }
            else
            {
                app
                    .UseStatusCodePagesWithReExecute(Error.ErrorPage)
                    .UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultRoute();
                    endpoints.MapDefaultAreaRoute();
                    endpoints.MapDefaultControllerRoute();

                    endpoints.MapHub<ChatHub>("/chat");

                    endpoints.MapRazorPages();
                });
        }
    }
}