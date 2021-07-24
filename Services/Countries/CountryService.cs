namespace MessiFinder.Services.Countries
{
    using System.Collections.Generic;
    using System.Globalization;

    public class CountryService : ICountryService
    {
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
                }
            }

            cultureList.Sort();
            return cultureList;
        }
    }
}
