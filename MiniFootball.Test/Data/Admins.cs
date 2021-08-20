namespace MiniFootball.Test.Data
{
    using MiniFootball.Data.Models;

    public static class Admins
    {
        public static Admin NewAdmin()
            => new Admin
            {
                Name = "TestUser",
                UserId = "TestId",
            };
    }
}
