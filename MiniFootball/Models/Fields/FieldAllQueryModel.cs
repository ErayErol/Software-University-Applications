namespace MiniFootball.Models.Fields
{
    using System.Collections.Generic;
    using Services.Fields;

    public class FieldAllQueryModel
    {
        public int PlaygroundsPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string City { get; set; }

        public string SearchTerm { get; set; }

        public int TotalFields { get; set; }

        public Sorting Sorting { get; set; }

        public IEnumerable<string> Cities { get; set; }

        public IEnumerable<FieldServiceModel> Fields { get; set; }
    }
}
