namespace MessiFinder.Services.Users
{
    using Data;
    using Infrastructure;
    using System.Linq;
    using System.Security.Claims;

    public class UserService : IUserService
    {
        private readonly MessiFinderDbContext data;
        private readonly ClaimsPrincipal user;

        public UserService(MessiFinderDbContext data, ClaimsPrincipal user)
        {
            this.data = data;
            this.user = user;
        }

        public ClaimsPrincipal User()
            => this.user;

        public bool UserIsAdmin()
            => this.data
                .Admins
                .Any(d => d.UserId == this.user.Id());
    }
}
