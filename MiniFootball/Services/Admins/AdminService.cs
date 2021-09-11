namespace MiniFootball.Services.Admins
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;

    public class AdminService : IAdminService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;

        public AdminService(MiniFootballDbContext data,
                            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public bool IsAdmin(string userId)
            => data
                .Admins
                .Any(a => a.UserId == userId && a.IsPublic);

        public int IdByUser(string userId)
            => data
                .Admins
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .FirstOrDefault();

        public int Become(string name, string userId)
        {
            var admin = new Admin
            {
                Name = name,
                UserId = userId,
                IsPublic = false,
            };

            data.Admins.Add(admin);
            data.SaveChanges();

            return admin.Id;
        }

        public void ChangeVisibility(int id)
        {
            var admin = data.Admins.Find(id);
            admin.IsPublic = !admin.IsPublic;
            data.SaveChanges();
        }

        public List<AdminListingServiceModel> All()
        {
            return this.data.Admins
                   .ProjectTo<AdminListingServiceModel>(mapper)
                   .ToList();
        }
    }
}
