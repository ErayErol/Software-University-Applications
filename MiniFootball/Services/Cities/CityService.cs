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
            => this.data
                .Cities
                .FirstOrDefault(c => c.Name == name);

        public int Create(
            string name, 
            string countryName, 
            int adminId)
        {
            var city = this.City(name);

            if (city != null)
            {
                return 0;
            }

            city = new City
            {
                Name = name,
                Country= this.countries.Country(countryName),
                AdminId = adminId,
            };

            this.data.Cities.Add(city);
            this.data.SaveChanges();

            return city.Id;
        }
    }
}