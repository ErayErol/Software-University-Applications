namespace MiniFootball.Services.Cities
{
    using System.Linq;
    using Countries;
    using Data;
    using Data.Models;

    public class CityService : ICityService
    {
        private readonly MiniFootballDbContext data;
        private readonly ICountryService countries;

        public CityService(
            MiniFootballDbContext data, 
            ICountryService countries)
        {
            this.data = data;
            this.countries = countries;
        }

        public City City(string name)
            => data
                .Cities
                .FirstOrDefault(c => c.Name == name);

        public int Create(
            string name, 
            string countryName, 
            int adminId)
        {
            var city = City(name);

            if (city != null)
            {
                return 0;
            }

            city = new City
            {
                Name = name,
                Country= countries.Country(countryName),
                AdminId = adminId,
            };

            data.Cities.Add(city);
            data.SaveChanges();

            return city.Id;
        }
    }
}