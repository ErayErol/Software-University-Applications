namespace MiniFootball.Services.Games.Models
{
    public class GameDetailsServiceModel : GameListingServiceModel
    {
        public string Description { get; set; }

        public string FacebookUrl { get; set; }

        public int? NumberOfPlayers { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        public int Places { get; set; }

        public bool HasPlaces
            => IsHasPlaces;

        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public string UserId { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsUserAlreadyJoin { get; set; }

        private bool IsHasPlaces
            => Places > 0;
    }
}
