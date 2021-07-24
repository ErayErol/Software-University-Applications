namespace MessiFinder.Services.Admins
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Models.Admins;
    using Users;

    public class AdminService : IAdminService
    {
        private readonly MessiFinderDbContext data;
        private readonly IUserService userService;

        public AdminService(MessiFinderDbContext data, IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public void Become(BecomeAdminFormModel admin)
        {
            var userId = this.userService.User().Id();

            var adminData = new Admin
            {
                Name = admin.Name,
                PhoneNumber = admin.PhoneNumber,
                UserId = userId
            };

            this.data.Admins.Add(adminData);
            this.data.SaveChanges();
        }

        public int GetId()
            => this.data
                .Admins
                .Where(d => d.UserId == this.userService.User().Id())
                .Select(d => d.Id)
                .FirstOrDefault();
    }
}
