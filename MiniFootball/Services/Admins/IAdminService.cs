namespace MiniFootball.Services.Admins
{
    public interface IAdminService
    {
        public bool IsAdmin(string userId);

        public int IdByUser(string userId);
        
        int Become(string name, string phoneNumber, string userId);
    }
}
