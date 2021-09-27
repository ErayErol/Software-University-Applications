namespace MiniFootball.Test.Data
{
    using MiniFootball.Data.Models;

    public static class Fields
    {
        public static Field Avenue()
            => new Field
            {
                Name = "Avenue",
                CountryId = 1,
                CityId = 1,
                Description = "In the summer this place is number 1 to play mini football.",
                Address = "Ул. дунав 1 - в парка под супермаркет авеню",
                //PhotoPath = "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg",
                Cafe = true,
                ChangingRoom = true,
                Parking = true,
                Shower = true,
                PhoneNumber = "0888888889",
                AdminId = 1,
            };
    }
}
