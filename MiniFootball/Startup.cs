namespace MiniFootball
{
    using Data;
    using Data.Models;
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
    using AspNetCoreHero.ToastNotification;
    using Hubs;
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
            services.AddSignalR();

            services.AddMemoryCache();

            services.AddAutoMapper(typeof(Startup));

            services
                .AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDbContext<MiniFootballDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString(DefaultConnection)));

            services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            services.AddTransient<ClaimsPrincipal>(options =>
                options.GetService<IHttpContextAccessor>()?.HttpContext?.User);

            services
                .AddNotyf(config =>
                {
                    config.DurationInSeconds = 10;
                    config.IsDismissable = true;
                    config.Position = NotyfPosition.TopCenter;
                });

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
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
                    endpoints.MapDefaultAreaRoute();

                    endpoints
                        .MapControllerRoute(
                            name: "Admin Become",
                            pattern: "Admin/Admins/Become",
                            defaults: new { controller = "Admins", action = "Become" });

                    endpoints
                        .MapControllerRoute(
                        name: "Game Details",
                        pattern: "/Games/Details/{gameId}/{information}",
                        defaults: new { controller = "Games", action = "Details" });

                    endpoints
                        .MapControllerRoute(
                            name: "Game Delete",
                            pattern: "/Games/Delete/{gameId}/{information}",
                            defaults: new { controller = "Games", action = "Delete" });

                    endpoints
                        .MapControllerRoute(
                            name: "Game Edit",
                            pattern: "/Games/Edit/{gameId}/{information}",
                            defaults: new { controller = "Games", action = "Edit" });

                    endpoints
                        .MapControllerRoute(
                            name: "Field Details",
                            pattern: "/Fields/Details/{id}/{information}",
                            defaults: new { controller = "Fields", action = "Details" });

                    endpoints
                        .MapControllerRoute(
                            name: "Field Edit",
                            pattern: "/Fields/Edit/{id}/{information}",
                            defaults: new { controller = "Fields", action = "Edit" });

                    endpoints
                        .MapControllerRoute(
                            name: "Field Delete",
                            pattern: "/Fields/Delete/{id}/{information}",
                            defaults: new { controller = "Fields", action = "Delete" });

                    endpoints.MapHub<ChatHub>("/chat");

                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}