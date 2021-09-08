namespace MiniFootball.Services.Users
{
    using Areas.Identity.Pages.Account.Manage;
    using Data.Models;
    using Games.Models;

    public interface IUserService
    {
        GameUserInfoServiceModel UserInfo(string id);
        
        UserDetailsServiceModel UserDetails(string id);
        
        bool Edit(User user, IndexModel.InputModel input);
    }
}
