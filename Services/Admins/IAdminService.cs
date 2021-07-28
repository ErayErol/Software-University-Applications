namespace MessiFinder.Services.Admins
{
    public interface IAdminService
    {
        public bool IsAdmin(string userId);

        public int IdByUser(string userId);
    }
}
