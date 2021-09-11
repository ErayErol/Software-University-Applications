namespace MiniFootball.Services.Admins
{
    using System.Collections.Generic;

    public interface IAdminService
    {
        public bool IsAdmin(string userId);

        public int IdByUser(string userId);
        
        int Become(string name, string userId);

        void ChangeVisibility(int id);
        
        List<AdminListingServiceModel> All();
    }
}
