namespace MiniFootball.Test.Data
{
    using MiniFootball.Data.Models;

    public static class Cities
    {
        public static City Sofia()
            => new City
            {
                Name = "Sofia",
                CountryId = 24,
                AdminId = 1,
            };

        public static City Haskovo()
            => new City
            {
                Name = "Haskovo",
                CountryId = 1,
                AdminId = 1,
            };
    }
}
