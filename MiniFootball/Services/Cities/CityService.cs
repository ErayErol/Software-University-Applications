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

        public int CityIdByName(string name)
            => data
                .Cities
                .Where(c => c.Name == name)
                .Select(c => c.Id)
                .FirstOrDefault();

        public int Create(
            string name,
            string countryName,
            int adminId)
        {
            var id = CityIdByName(name);

            var city = data.Cities.FirstOrDefault(c => c.Id == id);

            if (city != null)
            {
                if (city.CountryId == countries.CountryIdByName(countryName))
                {
                    return 0;
                }
            }

            city = new City
            {
                Name = name,
                CountryId = countries.CountryIdByName(countryName),
                AdminId = adminId,
            };

            data.Cities.Add(city);
            data.SaveChanges();

            return city.Id;
        }
    }
}