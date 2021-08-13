namespace MiniFootball
{
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
    using Data;
    using Services.Admins;
    using Services.Countries;
    using Services.Fields;
    using Services.Games;
    using Services.Statistics;
    using Services.Users;
    using System.Security.Claims;
    using Services.Cities;

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
                .AddDbContext<MiniFootballDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services
                .AddDatabaseDeveloperPageExceptionFilter();

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

            services.AddAutoMapper(typeof(Startup));

            services.AddMemoryCache();

            services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            // If you don't have this, you cannot use ClaimsPrincipal in services
            services.AddTransient<ClaimsPrincipal>(options =>
                options.GetService<IHttpContextAccessor>()?.HttpContext?.User);

            services
                .AddTransient<ICityService, CityService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IGameService, GameService>()
                .AddTransient<IFieldService, FieldService>()
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<IStatisticsService, StatisticsService>()
                .AddTransient<ICountryService, CountryService>();

            // Cloudinary Setup
            //Cloudinary cloudinary = new Cloudinary(new Account(
            //    CloudName, // this.configuration["Cloudinary:CloudName"],
            //    ApiKey, //this.configuration["Cloudinary:ApiKey"],
            //    ApiSecret)); //this.configuration["Cloudinary:ApiSecret"]));
            //services.AddSingleton(cloudinary);
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}