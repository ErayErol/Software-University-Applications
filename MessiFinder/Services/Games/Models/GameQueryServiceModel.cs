namespace MessiFinder.Services.Games.Models
{
    using System.Collections.Generic;

    public class GameQueryServiceModel
    {
        public int GamesPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalGames { get; set; }

        public IEnumerable<GameListingServiceModel> Games { get; set; }
    }
}
