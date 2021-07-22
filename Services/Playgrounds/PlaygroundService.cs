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

        public IQueryable<PlaygroundAllViewModel> All()
            => this.data
                .Playgrounds
                .Select(p => new PlaygroundAllViewModel()
                {
                    Name = p.Name,
                    Country = p.Country,
                    Town = p.Town,
                    Address = p.Address,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                });

        public bool IsExist(PlaygroundListingViewModel gamePlaygroundModel)
            => this.data.Playgrounds.Any(p => p.Id == gamePlaygroundModel.PlaygroundId);

        public IEnumerable<PlaygroundListingViewModel> GetPlaygroundViewModels(string town, string country)
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
