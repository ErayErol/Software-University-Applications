namespace MiniFootball.Services.Users
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Games.Models;

    public class UserService : IUserService
    {
        private readonly MiniFootballDbContext data;

        public UserService(MiniFootballDbContext data) 
            => this.data = data;

        public GameUserInfoServiceModel UserInfo(string id) 
            => this.data
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

        public User User(string id)
            => this.data.Users.FirstOrDefault(u => u.Id == id);
    }
}