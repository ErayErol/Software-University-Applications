namespace MessiFinder.Services.Playgrounds
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Models;
    using Games;
    using Models;
    using Models.Playgrounds;

    public class PlaygroundService : IPlaygroundService
    {
        private readonly MessiFinderDbContext data;

        public PlaygroundService(MessiFinderDbContext data)
        {
            this.data = data;
        }

        public int Create(
            string name,
            string country,
            string town,
            string address,
            string imageUrl,
            string phoneNumber,
            bool parking,
            bool cafe,
            bool shower,
            bool changingRoom,
            string description,
            int adminId)
        {
            var playground = new Playground
            {
                Name = name,
                Country = country,
                Town = town,
                Address = address,
                ImageUrl = imageUrl,
                PhoneNumber = phoneNumber,
                Parking = parking,
                Cafe = cafe,
                Shower = shower,
                ChangingRoom = changingRoom,
                Description = description,
                AdminId = adminId
            };

            this.data.Playgrounds.Add(playground);
            this.data.SaveChanges();

            return playground.Id;
        }

        public bool IsSame(string name, string country, string town, string address)
            => this.data
                .Playgrounds
                .Any(p => p.Name == name &&
                          p.Country == country &&
                          p.Town == town &&
                          p.Address == address);

        public IEnumerable<string> Towns()
            => this.data
                .Playgrounds
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

        public IEnumerable<PlaygroundListingServiceModel> PlaygroundsListing(string town, string country)
            => this.data
                .Playgrounds
                .Where(x => x.Town == town && x.Country == country)
                .Select(x => new PlaygroundListingServiceModel
                {
                    PlaygroundId = x.Id,
                    Name = x.Name,
                }).ToList();

        public bool PlaygroundExist(int playgroundId)
            => this.data.Playgrounds.Any(p => p.Id == playgroundId);

        public PlaygroundQueryServiceModel All(
            string town, 
            string searchTerm, 
            GameSorting sorting, 
            int currentPage,
            int playgroundsPerPage)
        {
            var playgroundsQuery = this.data.Playgrounds.AsQueryable();

            if (string.IsNullOrWhiteSpace(town) == false)
            {
                playgroundsQuery = playgroundsQuery.Where(g => g.Town == town);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                playgroundsQuery = playgroundsQuery
                    .Where(g => g
                        .Name
                        .ToLower()
                        .Contains(searchTerm.ToLower()));
            }

            playgroundsQuery = sorting switch
            {
                GameSorting.Town => playgroundsQuery.OrderBy(g => g.Town),
                GameSorting.PlaygroundName => playgroundsQuery.OrderBy(g => g.Name),
                GameSorting.DateCreated or _ => playgroundsQuery.OrderBy(g => g.Id)
            };

            var totalPlaygrounds = playgroundsQuery.Count();

            var playgrounds = playgroundsQuery
                .Skip((currentPage - 1) * playgroundsPerPage)
                .Take(playgroundsPerPage)
                .Select(p => new PlaygroundServiceModel
                {
                    Town = p.Town,
                    Country = p.Country,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Address = p.Address,
                }).AsEnumerable();

            return new PlaygroundQueryServiceModel
            {
                Playgrounds = playgrounds,
                TotalPlaygrounds = totalPlaygrounds
            };
        }
    }
}
