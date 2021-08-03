namespace MiniFootball.Services.Admins
{
    using Data;
    using System.Linq;
    using Data.Models;

    public class AdminService : IAdminService
    {
        private readonly MiniFootballDbContext data;

        public AdminService(MiniFootballDbContext data)
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

        public int Become(string name, string phoneNumber, string userId)
        {
            var admin = new Admin
            {
                Name = name,
                PhoneNumber = phoneNumber,
                UserId = userId
            };

            this.data.Admins.Add(admin);
            this.data.SaveChanges();

            return admin.Id;
        }
    }
}
