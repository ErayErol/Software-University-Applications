namespace MiniFootball.Services.Admins
{
    public class AdminListingServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public bool IsPublic { get; set; }
    }
}
