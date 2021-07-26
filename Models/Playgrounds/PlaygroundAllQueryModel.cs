namespace MessiFinder.Models.Playgrounds
{
    using System.Collections.Generic;

    public class PlaygroundAllQueryModel
    {
        public const int GamesPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public string Town { get; set; }

        public string SearchTerm { get; set; }

        public int TotalPlaygrounds { get; set; }

        public GameSorting Sorting { get; set; }

        public IEnumerable<string> Towns { get; set; }

        public IEnumerable<PlaygroundAllViewModel> Playgrounds { get; set; }
    }
}
