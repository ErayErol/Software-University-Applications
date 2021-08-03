namespace MiniFootball.Models.Playgrounds
{
    using System.Collections.Generic;
    using Services.Playgrounds;

    public class PlaygroundAllQueryModel
    {
        public int PlaygroundsPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string Town { get; set; }

        public string SearchTerm { get; set; }

        public int TotalPlaygrounds { get; set; }

        public GameSorting Sorting { get; set; }

        public IEnumerable<string> Towns { get; set; }

        public IEnumerable<PlaygroundServiceModel> Playgrounds { get; set; }
    }
}
