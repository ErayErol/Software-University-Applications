namespace MiniFootball.Services.Users
{
    using System;
    using System.IO;
    using Data;
    using Games.Models;
    using System.Linq;
    using Areas.Identity.Pages.Account.Manage;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Microsoft.AspNetCore.Hosting;

    public class UserService : IUserService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;
        private readonly IWebHostEnvironment hostEnvironment;


        public UserService(MiniFootballDbContext data, 
                           IMapper mapper, 
                           IWebHostEnvironment hostEnvironment)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
            this.hostEnvironment = hostEnvironment;
        }

        public GameUserInfoServiceModel UserInfo(string id)
            => data
                .Users
                .Where(x => x.Id == id)
                .Select(x => new GameUserInfoServiceModel
                {
                    UserId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    NickName = x.NickName,
                    PhoneNumber = x.PhoneNumber,
                    PhotoPath = x.PhotoPath,
                })
                .FirstOrDefault();

        public UserDetailsServiceModel UserDetails(string id)
        {
            var user = data
                .Users
                .Where(u => u.Id == id)
                .ProjectTo<UserDetailsServiceModel>(mapper)
                .FirstOrDefault();

            if (user != null)
            {
                CorrectAge(user);
            }

            return user;
        }

        public bool Edit(User user, IndexModel.InputModel input)
        {
            var userId = this.data
                .Users
                .Where(u => u.Id == user.Id)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (string.Empty.Equals(userId))
            {
                return false;
            }

            user.PhoneNumber = input.PhoneNumber;
            user.Email = input.Email;
            user.FirstName = input.FirstName;
            user.LastName = input.LastName;
            user.NickName = input.NickName;
            user.PhoneNumber = input.PhoneNumber;
            user.Birthdate = input.Birthdate;

            if (input.Photo != null)
            {
                input.PhotoPath = ProcessUploadFile(input);
                user.PhotoPath = input.PhotoPath;
            }
            else
            {
                input.PhotoPath = user.PhotoPath;
            }

            data.Users.Update(user);
            data.SaveChanges();

            return true;
        }


        private string ProcessUploadFile(IndexModel.InputModel inputModel)
        {
            string uniqueFileName = null;

            if (inputModel.Photo != null)
            {
                var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + inputModel.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                inputModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }

        private static void CorrectAge(UserDetailsServiceModel userDetails)
        {
            var today = DateTime.Today;
            userDetails.Age = (today.Year - userDetails.Birthdate.Date.Year) - 1;

            if (userDetails.Age > 100)
            {
                userDetails.Age = 0;
            }

            if (today.Month > userDetails.Birthdate.Month)
            {
                userDetails.Age += 1;
            }
            else if (today.Month == userDetails.Birthdate.Month)
            {
                if (today.Day >= userDetails.Birthdate.Day)
                {
                    userDetails.Age += 1;
                }
            }
        }
    }
}