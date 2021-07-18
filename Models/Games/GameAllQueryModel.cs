namespace MessiFinder.Models.Games
{
    using System.Collections.Generic;

    public class GameAllQueryModel
    {
        public const int GamesPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public string Town { get; set; }

        public string SearchTerm { get; set; }

        public int TotalGames { get; set; }

        public GameSorting Sorting { get; set; }

        public IEnumerable<string> Towns { get; set; }

        public IEnumerable<GameListingViewModel> Games { get; set; }
    }
}
