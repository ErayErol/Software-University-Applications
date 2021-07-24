namespace MessiFinder.Services.Playgrounds
{
    using System.Collections.Generic;
    using Data;
    using Data.Models;
    using Models.Playgrounds;
    using System.Linq;
    using Models.Games;

    public class PlaygroundService : IPlaygroundService
    {
        private readonly MessiFinderDbContext data;

        public PlaygroundService(MessiFinderDbContext data)
        {
            this.data = data;
        }

        public void Create(PlaygroundCreateFormModel playgroundModel, int adminId)
        {
            var playground = new Playground()
            {
                Name = playgroundModel.Name,
                Country = playgroundModel.Country,
                Town = playgroundModel.Town,
                Address = playgroundModel.Address,
                ImageUrl = playgroundModel.ImageUrl,
                PhoneNumber = playgroundModel.PhoneNumber,
                Parking = playgroundModel.Parking,
                Cafe = playgroundModel.Cafe,
                Shower = playgroundModel.Shower,
                ChangingRoom = playgroundModel.ChangingRoom,
                Description = playgroundModel.Description,
                AdminId = adminId
            };

            this.data.Playgrounds.Add(playground);
            this.data.SaveChanges();
        }

        public bool CheckForSamePlayground(PlaygroundCreateFormModel playgroundModel)
            => this.data.Playgrounds
                .Any(p =>
                    p.Name == playgroundModel.Name &&
                    p.Country == playgroundModel.Country &&
                    p.Town == playgroundModel.Town &&
                    p.Address == playgroundModel.Address);

        public PlaygroundAllQueryModel All(PlaygroundAllQueryModel query)
        {
            var playgroundsQuery = this.data.Playgrounds.AsQueryable();

            if (string.IsNullOrWhiteSpace(query.Town) == false)
            {
                playgroundsQuery = playgroundsQuery.Where(g => g.Town == query.Town);
            }

            if (string.IsNullOrWhiteSpace(query.SearchTerm) == false)
            {
                playgroundsQuery = playgroundsQuery
                    .Where(g => g
                        .Name
                        .ToLower()
                        .Contains(query.SearchTerm.ToLower()));
            }

            playgroundsQuery = query.Sorting switch
            {
                GameSorting.Town => playgroundsQuery.OrderBy(g => g.Town),
                GameSorting.PlaygroundName => playgroundsQuery.OrderBy(g => g.Name),
                GameSorting.DateCreated or _ => playgroundsQuery.OrderBy(g => g.Id)
            };

            var totalPlaygrounds = playgroundsQuery.Count();

            var playgrounds = playgroundsQuery
                .Skip((query.CurrentPage - 1) * PlaygroundAllQueryModel.GamesPerPage)
                .Take(PlaygroundAllQueryModel.GamesPerPage)
                .Select(p => new PlaygroundAllViewModel()
                {
                    Town = p.Town,
                    Country = p.Country,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Address = p.Address,
                }).AsEnumerable();

            var towns = this.data
                .Playgrounds
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

            query.TotalPlaygrounds = totalPlaygrounds;
            query.Playgrounds = playgrounds;
            query.Towns = towns;

            return query;
        }

        public bool IsExist(PlaygroundListingViewModel gamePlaygroundModel)
            => this.data.Playgrounds.Any(p => p.Id == gamePlaygroundModel.PlaygroundId);

        public IEnumerable<PlaygroundListingViewModel> PlaygroundViewModels(string town, string country)
            => this.data
                .Playgrounds
                .Where(x => x.Town == town && x.Country == country)
                .Select(x => new PlaygroundListingViewModel
                {
                    PlaygroundId = x.Id,
                    Name = x.Name,
                }).ToList();
    }
}
