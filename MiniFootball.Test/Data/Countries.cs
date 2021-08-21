namespace MiniFootball.Test.Data
{
    using MiniFootball.Data.Models;

    public static class Countries
    {
        public static Country NewCountry()
            => new Country
            {
                Name = "Bulgaria",
            };
    }
}
