namespace MiniFootball.Services.Fields
{
    using System.Collections.Generic;

    public class FieldFormServiceModel : FieldDetailServiceModel
    {
        public int CountryId { get; set; }
        public IEnumerable<string> Countries { get; set; }

        public int CityId { get; set; }
    }
}
