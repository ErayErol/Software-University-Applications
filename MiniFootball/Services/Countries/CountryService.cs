namespace MiniFootball.Services.Countries
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Data.Models;
    using Microsoft.Extensions.Caching.Memory;

    using static WebConstants;

    public class CountryService : ICountryService
    {
        private const string latestGamesCacheKey = "AllCountriesCacheKey";
        private List<string> allCountries;

        private readonly MiniFootballDbContext data;
        private readonly IMemoryCache cache;

        public CountryService(
            MiniFootballDbContext data,
            IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
            this.allCountries = new List<string>();
        }

        public List<string> All()
        {
            this.allCountries = this.cache.Get<List<string>>(latestGamesCacheKey);

            if (allCountries == null)
            {
                allCountries = this.data.Countries.Select(country => country.Name).ToList();
                allCountries.Sort();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(365));

                this.cache.Set(latestGamesCacheKey, allCountries, cacheOptions);
            }

            return allCountries;
        }

        public void SaveAll()
        {
            var cultureList = new List<string>();

            var getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var getCulture in getCultureInfo)
            {
                var getRegionInfo = new RegionInfo(getCulture.LCID);

                if (cultureList.Contains(getRegionInfo.EnglishName) == false)
                {
                    cultureList.Add(getRegionInfo.EnglishName);
                    this.data.Countries.Add(new Country { Name = getRegionInfo.EnglishName });
                    this.data.SaveChanges();
                }
            }
        }

        public string Name(int id)
            => this.data
                .Countries
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefault();

        public Country Country(string name)
            => this.data
                .Countries
                .FirstOrDefault(c => c.Name == name);

        public IEnumerable<string> Cities(string countryName)
        {
            return this.data
                .Cities
                .Where(c => c.Country.Name == countryName)
                .Select(c => c.Name);
        }
    }
}