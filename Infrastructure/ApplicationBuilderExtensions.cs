namespace MessiFinder.Infrastructure
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopeService = app.ApplicationServices.CreateScope();

            var data = scopeService.ServiceProvider.GetService<MessiFinderDbContext>();
            var passwordHasher = scopeService.ServiceProvider.GetService<IPasswordHasher<User>>();

            data?.Database.Migrate();

            Seeds(data, passwordHasher);

            return app;
        }

        private static void Seeds(MessiFinderDbContext data, IPasswordHasher<User> passwordHasher)
        {
            if (data.Users.Any() == false)
            {
                for (var i = 1; i <= 3; i++)
                {
                    var applicationUser = new User
                    {
                        UserName = $"zwp{i}@gmail.com",
                        Email = $"zwp{i}@gmail.com",
                        NormalizedUserName = $"zwp{i}@gmail.com"
                    };

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
                    PhoneNumber = "0886911492",
                    Name = "zwpAdmin",
                    UserId = user?.Id,
                });

                data.SaveChanges();
            }

            if (data.Playgrounds.Any() == false)
            {
                var admin = data.Admins.FirstOrDefault(x => x.Name == "zwpAdmin");

                data.Playgrounds.AddRange(new Playground
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
                    AdminId = admin.Id,
                }, new Playground
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
                    AdminId = admin.Id,
                }, new Playground
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
                    AdminId = admin.Id,
                }, new Playground
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
                    AdminId = admin.Id,
                }, new Playground
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
                    AdminId = admin.Id,
                });
            }

            data.SaveChanges();
        }
    }
}
