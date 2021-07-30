namespace MessiFinder.Services.Games.Models
{
    public class GameDetailsServiceModel : GameListingServiceModel
    {
        public string Description { get; set; }

        public int? NumberOfPlayers { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public string UserId { get; set; }
    }
}
