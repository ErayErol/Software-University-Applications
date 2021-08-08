namespace MiniFootball.Services.Games.Models
{
    public class GameSeePlayersServiceModel
    {
        public string UserId { get; set; }

        public string ImageUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsCreator { get; set; }
    }
}
