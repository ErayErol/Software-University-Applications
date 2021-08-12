namespace MiniFootball.Services.Users
{
    using Data.Models;
    using Games.Models;

    public interface IUserService
    {
        GameUserInfoServiceModel UserInfo(string id);
        
        User User(string  id);
    }
}
