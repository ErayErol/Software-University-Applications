namespace MiniFootball.Services.Countries
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Data.Models;

    public class CountryService : ICountryService
    {
        private readonly MiniFootballDbContext data;

        public CountryService(MiniFootballDbContext data)
        {
            this.data = data;
        }

        // TODO: Do it void
        public IEnumerable<string> All()
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

            cultureList.Sort();
            return cultureList;
        }

        public string Name(int id)
            => this.data
                .Countries
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefault();
    }
}