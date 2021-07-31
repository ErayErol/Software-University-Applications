namespace MessiFinder.Models.Games
{
    using Services.Games.Models;
    using System.Collections.Generic;

    public class GameAllQueryModel
    {
        public int GamesPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string Town { get; set; }

        public string SearchTerm { get; set; }

        public int TotalGames { get; set; }

        public GameSorting Sorting { get; set; }

        public IEnumerable<string> Towns { get; set; }

        public IEnumerable<GameListingServiceModel> Games { get; set; }
    }
}
