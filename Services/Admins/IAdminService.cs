namespace MessiFinder.Services.Admins
{
    using Models.Admins;

    public interface IAdminService
    {
        void Become(BecomeAdminFormModel admin);

        int GetId();
    }
}
