namespace MessiFinder.Services.Users
{
    using System.Security.Claims;

    public interface IUserService
    {
        ClaimsPrincipal GetUser();
        
        bool UserIsAdmin();
    }
}
