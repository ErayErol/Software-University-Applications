namespace MiniFootball.Models.Fields
{
    using System.Collections.Generic;
    using Services.Fields;

    public class FieldAllQueryModel
    {
        public int PlaygroundsPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string Town { get; set; }

        public string SearchTerm { get; set; }

        public int TotalFields { get; set; }

        public GameSorting Sorting { get; set; }

        public IEnumerable<string> Towns { get; set; }

        public IEnumerable<FieldServiceModel> Fields { get; set; }
    }
}
