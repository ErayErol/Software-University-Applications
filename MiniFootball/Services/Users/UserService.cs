namespace MiniFootball.Services.Users
{
    using System;
    using Data;
    using Games.Models;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public class UserService : IUserService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;


        public UserService(MiniFootballDbContext data, 
                           IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;

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
                    ImageUrl = x.ImageUrl,
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

        private static void CorrectAge(UserDetailsServiceModel userDetails)
        {
            var today = DateTime.Today;
            userDetails.Age = (today.Year - userDetails.Birthdate.Date.Year) - 1;

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