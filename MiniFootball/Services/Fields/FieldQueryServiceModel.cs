namespace MiniFootball.Services.Fields
{
    using System.Collections.Generic;

    public class FieldQueryServiceModel
    {
        public int TotalFields { get; set; }

        public IEnumerable<FieldServiceModel> Fields { get; set; }
    }
}
