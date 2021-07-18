namespace MessiFinder.Utilities
{
    using System.Collections.Generic;
    using System.Globalization;

    public static class Countries
    {
        public static IEnumerable<string> GetAll()
        {
            var cultureList = new List<string>();

            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);

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
