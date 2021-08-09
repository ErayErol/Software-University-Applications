namespace MiniFootball.Models.Api
{
    public class AllGamesApiRequestModel
    {
        public string City { get; init; }

        public string SearchTerm { get; init; }

        public Sorting Sorting { get; init; }

        public int GamesPerPage { get; init; } = 10;

        public int CurrentPage { get; init; } = 1;
    }
}
