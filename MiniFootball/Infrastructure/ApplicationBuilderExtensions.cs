namespace MiniFootball.Infrastructure
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Countries;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            Seeds(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            services
                .GetRequiredService<MiniFootballDbContext>()
                .Database
                .Migrate();
        }

        private static void Seeds(IServiceProvider services)
        {
            var data = services.GetService<MiniFootballDbContext>();

            CreateCountries(data, services);

            CreateUsers(data, services);

            CreateAdmins(data);

            CreateFields(data);

            CreateGames(data);

            CreateRoleManager(services);

            data?.SaveChanges();
        }

        private static void CreateCountries(MiniFootballDbContext data, IServiceProvider services)
        {
            if (data.Countries.Any() == false)
            {
                services
                    .GetService<ICountryService>()?
                    .SaveAll();
            }
        }
        
        private static void CreateUsers(MiniFootballDbContext data, IServiceProvider services)
        {
            if (data.Users.Any() == false)
            {
                for (var i = 1; i <= 2; i++)
                {
                    var name = i == 1
                        ? "admin"
                        : "user";

                    var applicationUser = new User
                    {
                        UserName = $"{name}@{name}.com",
                        Email = $"{name}@{name}.com",
                        NormalizedUserName = $"{name}@{name}.com",
                        FirstName = $"FirstName-{name}",
                        LastName = $"LastName-{name}",
                        NickName = $"NickName-{name}",
                        PhoneNumber = $"088691149",
                        Birthdate = DateTime.ParseExact(
                            $"199{i}-0{i}-0{i} 1{i}:{i}0:52,531",
                            "yyyy-MM-dd HH:mm:ss,fff",
                            CultureInfo.InvariantCulture),
                    };

                    applicationUser.ImageUrl = i switch
                    {
                        1 => "https://thumbs.dreamstime.com/b/admin-sign-laptop-icon-stock-vector-166205404.jpg",
                        2 => "https://amzsummits.com/wp-content/uploads/2019/05/Ferry-Vermeulen.jpeg",
                        _ => applicationUser.ImageUrl
                    };

                    data.Users.Add(applicationUser);
                    var passwordHasher = services.GetService<IPasswordHasher<User>>();
                    var hashedPassword = passwordHasher?.HashPassword(applicationUser, "123456");
                    applicationUser.SecurityStamp = Guid.NewGuid().ToString();
                    applicationUser.PasswordHash = hashedPassword;
                    data.SaveChanges();
                }
            }
        }
        
        private static void CreateAdmins(MiniFootballDbContext data)
        {
            if (data.Admins.Any() == false)
            {
                var user = data.Users.FirstOrDefault(x => x.UserName == "admin@admin.com");

                data.Admins.Add(new Admin
                {
                    Name = "admin",
                    UserId = user?.Id,
                });

                data.SaveChanges();
            }
        }
        
        private static void CreateFields(MiniFootballDbContext data)
        {
            if (data.Fields.Any() == false)
            {
                CreateCities(data);

                AddFields(data);
            }
        }

        private static void AddFields(MiniFootballDbContext data)
        {
            data.Fields.AddRange(
                new Field
                {
                    Name = "Bogorodica",
                    CountryId = data.Countries.Where(c => c.Name == "Bulgaria").Select(c => c.Id).FirstOrDefault(),
                    CityId = data.Cities.Where(c => c.Name == "Haskovo").Select(c => c.Id).FirstOrDefault(),
                    Description = "This place is very good to play mini football, we have two fields, a small and a large.",
                    Address = "Зад монумент света богородица",
                    PhotoPath = "bogorodica.jpg",
                    Cafe = false,
                    ChangingRoom = false,
                    Parking = true,
                    Shower = false,
                    PhoneNumber = "0888888881",
                    AdminId = 1,
                },
                new Field
                {
                    Name = "Avenue",
                    CountryId = data.Countries.Where(c => c.Name == "Bulgaria").Select(c => c.Id).FirstOrDefault(),
                    CityId = data.Cities.Where(c => c.Name == "Haskovo").Select(c => c.Id).FirstOrDefault(),
                    Description = "In the summer this place is number 1 to play mini football.",
                    Address = "ул. Дунав 1 - в парка под супермаркет авеню",
                    PhotoPath = "avenue.jpeg",
                    Cafe = true,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888889",
                    AdminId = 1,
                },
                new Field
                {
                    Name = "Kortove",
                    CountryId = data.Countries.Where(c => c.Name == "Bulgaria").Select(c => c.Id).FirstOrDefault(),
                    CityId = data.Cities.Where(c => c.Name == "Haskovo").Select(c => c.Id).FirstOrDefault(),
                    Description =
                        "In the winter this place is number 1 to play mini football, because the players play inside when it is very cold.",
                    Address = "След Хотел Европа - до тенис кортовете",
                    PhotoPath = "kortove.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888888",
                    AdminId = 1,
                },
                new Field
                {
                    Name = "Rooftop Football",
                    CountryId = data.Countries.Where(c => c.Name == "Japan").Select(c => c.Id).FirstOrDefault(),
                    CityId = data.Cities.Where(c => c.Name == "Tokyo").Select(c => c.Id).FirstOrDefault(),
                    Description = "In the summer this place is number 1 to play mini football in Tokyo.",
                    Address = "Some Japanese Address.",
                    PhotoPath = "tokyo.jpg",
                    Cafe = true,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = false,
                    PhoneNumber = "04444444444",
                    AdminId = 1,
                },
                new Field
                {
                    Name = "Optimum",
                    CountryId = data.Countries.Where(c => c.Name == "Bulgaria").Select(c => c.Id).FirstOrDefault(),
                    CityId = data.Cities.Where(c => c.Name == "Plovdiv").Select(c => c.Id).FirstOrDefault(),
                    Description = "In summer and winter this place is number 1 to play mini football in Plovdiv.",
                    Address = "бул. „Асеновградско шосе",
                    PhotoPath = "optimum.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888886",
                    AdminId = 1,
                },
                new Field
                {
                    Name = "Avangard Fitness",
                    CountryId = data.Countries.Where(c => c.Name == "Bulgaria").Select(c => c.Id).FirstOrDefault(),
                    CityId = data.Cities.Where(c => c.Name == "Plovdiv").Select(c => c.Id).FirstOrDefault(),
                    Description = "You can workout in fitness and then play football with friends.",
                    Address = "жк. Тракия 96-Д, 4023 кв. Капитан Бураго",
                    PhotoPath = "avangard.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888885",
                    AdminId = 1,
                });

            data.SaveChanges();
        }

        private static void CreateCities(MiniFootballDbContext data)
        {
            if (data.Cities.Any() == false)
            {
                data.Cities.AddRange(
                    new City
                    {
                        CountryId = data
                            .Countries
                            .Where(c => c.Name == "Bulgaria")
                            .Select(c => c.Id)
                            .FirstOrDefault(),
                        Name = "Haskovo",
                        AdminId = 1,
                    },
                    new City
                    {
                        CountryId = data
                            .Countries
                            .Where(c => c.Name == "Bulgaria")
                            .Select(c => c.Id)
                            .FirstOrDefault(),
                        Name = "Plovdiv",
                        AdminId = 1,
                    },
                    new City
                    {
                        CountryId = data
                            .Countries
                            .Where(c => c.Name == "Bulgaria")
                            .Select(c => c.Id)
                            .FirstOrDefault(),
                        Name = "Sofia",
                        AdminId = 1,
                    },
                    new City
                    {
                        CountryId = data
                            .Countries
                            .Where(c => c.Name == "Japan")
                            .Select(c => c.Id)
                            .FirstOrDefault(),
                        Name = "Tokyo",
                        AdminId = 1,
                    }
                );

                data.SaveChanges();
            }
        }

        private static void CreateGames(MiniFootballDbContext data)
        {
            if (data.Games.Any() == false)
            {
                var today = DateTime.Today;
                var day = today.Day + 2;
                var month = today.Month;
                var year = today.Year;

                if (today.Day > 24)
                {
                    day = 2;
                    month += 1;
                    if (month >= 12)
                    {
                        month = 1;
                        year += 1;
                    }
                }

                var adminUserId = data
                    .Admins
                    .FirstOrDefault(a => a.Id == 1)?
                    .UserId;
                
                var phoneNumber = data.Users
                    .Where(u => u.Id == adminUserId)
                    .Select(u => u.PhoneNumber)
                    .FirstOrDefault();

                data.Games.AddRange(
                    new Game
                    {
                        AdminId = 1,
                        FieldId = 3,
                        Date = DateTime.ParseExact($"{month:D2}/{day:D2}/{year}", "d", CultureInfo.InvariantCulture),
                        Time = 20,
                        Ball = true,
                        Jerseys = true,
                        Description = "Just friendly game. It will be fun!",
                        NumberOfPlayers = 12,
                        Places = 12,
                        HasPlaces = true,
                        FacebookUrl = "https://www.facebook.com/profile.php?id=100001781550068",
                        PhoneNumber = phoneNumber,
                    },
                    new Game
                    {
                        AdminId = 1,
                        FieldId = 5,
                        Date = DateTime.ParseExact($"{month:D2}/{(day + 2):D2}/{year}", "d", CultureInfo.InvariantCulture),
                        Time = 19,
                        Ball = true,
                        Jerseys = true,
                        Description = "The level of the game will be one step more than the amateur.",
                        NumberOfPlayers = 8,
                        Places = 8,
                        HasPlaces = true,
                        FacebookUrl = "https://www.facebook.com/profile.php?id=100001781550068",
                        PhoneNumber = phoneNumber,
                    });

                data.SaveChanges();
            }
        }
        
        private static void CreateRoleManager(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(GlobalConstant.Manager.ManagerRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole
                    {
                        Name = GlobalConstant.Manager.ManagerRoleName
                    };

                    await roleManager.CreateAsync(role);

                    const string managerEmail = "manager@manager.com";
                    const string managerPassword = "123456";

                    var user = new User
                    {
                        Email = managerEmail,
                        UserName = managerEmail,
                        FirstName = "FirstName-manager",
                        LastName = "LastName-manager",
                        NickName = "NickName-manager",
                        PhoneNumber = "0886911492",
                        ImageUrl = "https://thumbs.dreamstime.com/b/manager-38039871.jpg",
                        Birthdate = DateTime.ParseExact(
                            "1995-09-30 14:00:52,531",
                            "yyyy-MM-dd HH:mm:ss,fff",
                            CultureInfo.InvariantCulture),
                    };

                    await userManager.CreateAsync(user, managerPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}