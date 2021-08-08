namespace MiniFootball.Services.Users
{
    using Games.Models;

    public interface IUserService
    {
        GameUserInfoServiceModel UserInfo(string id);
    }
}
