namespace MiniFootball.Models.Api
{
    public class AllGamesApiRequestModel
    {
        public string Town { get; init; }

        public string SearchTerm { get; init; }

        public GameSorting Sorting { get; init; }

        public int GamesPerPage { get; init; } = 10;

        public int CurrentPage { get; init; } = 1;
    }
}
