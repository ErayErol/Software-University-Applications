namespace MiniFootball.Infrastructure
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using static Areas.Manager.ManagerConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            var data = serviceScope.ServiceProvider.GetService<MiniFootballDbContext>();
            var passwordHasher = serviceScope.ServiceProvider.GetService<IPasswordHasher<User>>();

            MigrateDatabase(services);

            // TODO: Refactor this
            Seeds(data, passwordHasher);
            SeedManager(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<MiniFootballDbContext>();

            data.Database.Migrate();
        }

        private static void SeedManager(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(ManagerRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = ManagerRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@msf.com";
                    const string adminPassword = "admin@msf.com";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FirstName = adminEmail,
                        LastName = adminEmail,
                        NickName = AreaName,
                        PhoneNumber = "0886911492",
                        ImageUrl = "https://thumbs.dreamstime.com/b/manager-38039871.jpg",
                        Birthdate = DateTime.ParseExact("2019-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture),
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void Seeds(MiniFootballDbContext data, IPasswordHasher<User> passwordHasher)
        {
            if (data.Users.Any() == false)
            {
                for (var i = 1; i <= 3; i++)
                {
                    var applicationUser = new User
                    {
                        UserName = $"zwp{i}@gmail.com",
                        Email = $"zwp{i}@gmail.com",
                        NormalizedUserName = $"zwp{i}@gmail.com",
                        FirstName = $"zwp{i}@gmail.com",
                        LastName = $"zwp{i}@gmail.com",
                        NickName = $"zwp{i}@gmail.com",
                        PhoneNumber = $"zwp{i}@gmail.com",
                        Birthdate = DateTime.ParseExact("2009-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture),
                    };

                    if (i == 1)
                    {
                        applicationUser.ImageUrl =
                            "https://thumbs.dreamstime.com/b/admin-sign-laptop-icon-stock-vector-166205404.jpg";
                    }
                    else if (i == 2)
                    {
                        applicationUser.ImageUrl = "https://www.seekpng.com/png/full/356-3562377_personal-user.png";
                    }
                    else if (i == 3)
                    {
                        applicationUser.ImageUrl = "https://www.pngfind.com/pngs/m/470-4703547_icon-user-icon-hd-png-download.png";
                    }

                    data.Users.Add(applicationUser);

                    var hashedPassword = passwordHasher.HashPassword(applicationUser, $"zwp{i}@gmail.com");
                    applicationUser.SecurityStamp = Guid.NewGuid().ToString();
                    applicationUser.PasswordHash = hashedPassword;

                    data.SaveChanges();
                }
            }

            if (data.Admins.Any() == false)
            {
                var user = data.Users.FirstOrDefault(x => x.UserName == "zwp1@gmail.com");

                data.Admins.Add(new Admin
                {
                    Name = "zwpAdmin",
                    UserId = user?.Id,
                });

                data.SaveChanges();
            }

            if (data.Fields.Any() == false)
            {
                var admin = data.Admins.FirstOrDefault(x => x.Name == "zwpAdmin");

                // TODO: Add Players and Teams

                data.Fields.AddRange(new Field
                {
                    Name = "Avenue",
                    Country = "Bulgaria",
                    Town = "Haskovo",
                    Description = "In the summer this place is number 1 to play mini football.",
                    Address = "ул. Дунав 1 - в парка под супермаркет авеню",
                    ImageUrl = "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg",
                    Cafe = true,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888889",
                }, new Field
                {
                    Name = "Kortove",
                    Country = "Bulgaria",
                    Town = "Haskovo",
                    Description = "In the winter this place is number 1 to play mini football, because the players play inside.",
                    Address = "След Хотел Европа - до тенис кортовете",
                    ImageUrl = "https://tennishaskovo.com/uploads/galerii/baza_kenana/44.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888888",
                }, new Field
                {
                    Name = "Yildizlar",
                    Country = "Turkey",
                    Town = "Edirne",
                    Description = "In the summer this place is number 1 to play mini football in Edirne.",
                    Address = "Ilk Okullun yaninda.",
                    ImageUrl = "https://hotel-evrika.com/wp-content/uploads/2019/12/VIK_6225-1024x683.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888887",
                }, new Field
                {
                    Name = "Optimum",
                    Country = "Bulgaria",
                    Town = "Plovdiv",
                    Description = "In summer and winter this place is number 1 to play mini football in Plovdiv.",
                    Address = "бул. „Асеновградско шосе",
                    ImageUrl = "https://imgrabo.com/pics/guide/900x600/20150901162641_20158.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888886",
                }, new Field
                {
                    Name = "Avangard Fitness",
                    Country = "Bulgaria",
                    Town = "Plovdiv",
                    Description = "You can workout in fitness and then play football with friends.",
                    Address = "жк. Тракия 96-Д, 4023 кв. Капитан Бураго",
                    ImageUrl = "https://fitness-avantgarde.com/sites/default/files/img_8189.jpg",
                    Cafe = false,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888885",
                });

                data.SaveChanges();
            }

            if (data.Games.Any() == false)
            {
                data.Games.Add(new Game
                {
                    AdminId = 1,
                    FieldId = 1,
                    Date = DateTime.ParseExact("04/08/2021 21:00", "g", CultureInfo.InvariantCulture),
                    Ball = true,
                    Jerseys = true,
                    Description = "adasdasdadsadasdasdasdaasda",
                    NumberOfPlayers = 12,
                    Places = 12,
                    HasPlaces = true,
                    PhoneNumber = "0886911492",
                    FacebookUrl = "https://www.facebook.com/profile.php?id=100001781550068",
                });
            }

            data.SaveChanges();
        }
    }
}
