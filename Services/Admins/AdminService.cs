namespace MessiFinder.Services.Admins
{
    using Data;
    using System.Linq;

    public class AdminService : IAdminService
    {
        private readonly MessiFinderDbContext data;

        public AdminService(MessiFinderDbContext data)
        {
            this.data = data;
        }

        public bool IsAdmin(string userId)
            => this.data
                .Admins
                .Any(d => d.UserId == userId);

        public int IdByUser(string userId)
            => this.data
                .Admins
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
    }
}
