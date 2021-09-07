namespace MiniFootball.Services.Fields
{
    using Data.Models;

    public class FieldListingServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public string Address { get; set; }

        public string PhotoPath { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }
    }
}
