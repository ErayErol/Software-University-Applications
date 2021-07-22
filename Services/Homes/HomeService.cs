namespace MessiFinder.Services.Homes
{
    using Data;
    using Models.Home;
    using System.Linq;

    public class HomeService : IHomeService
    {
        private readonly MessiFinderDbContext data;

        public HomeService(MessiFinderDbContext data)
        {
            this.data = data;
        }

        public IndexViewModel Index()
        {
            var games = this.data
                .Games
                .Select(p => new GameIndexViewModel()
                {
                    Id = p.Id,
                    Playground = p.Playground,
                    Date = p.Date,
                })
                .OrderByDescending(g => g.Id)
                .Take(3)
                .ToList();

            var totalGames = this.data.Games.Count();
            var totalPlaygrounds = this.data.Playgrounds.Count();

            return new IndexViewModel
            {
                Games = games,
                TotalGames = totalGames,
                TotalPlaygrounds = totalPlaygrounds,
                TotalUsers = this.data.Users.Count(),
            };
        }
    }
}
