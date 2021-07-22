namespace MessiFinder.Services.Games
{
    using Data;
    using Data.Models;
    using Models.Games;
    using System.Linq;

    public class GameService : IGameService
    {
        private readonly MessiFinderDbContext data;

        public GameService(MessiFinderDbContext data)
        {
            this.data = data;
        }

        public void Create(GameCreateFormModel gameCreateModel, int adminId)
        {
            var game = new Game()
            {
                PlaygroundId = gameCreateModel.PlaygroundId,
                Description = gameCreateModel.Description,
                Date = gameCreateModel.Date.Value,
                NumberOfPlayers = gameCreateModel.NumberOfPlayers.Value,
                WithGoalkeeper = gameCreateModel.WithGoalkeeper,
                Ball = gameCreateModel.Ball,
                Jerseys = gameCreateModel.Jerseys,
                AdminId = adminId,
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();
        }

        public GameAllQueryModel All(GameAllQueryModel query)
        {
            var gamesQuery = this.data.Games.AsQueryable();

            if (string.IsNullOrWhiteSpace(query.Town) == false)
            {
                gamesQuery = gamesQuery.Where(g => g.Playground.Town == query.Town);
            }

            if (string.IsNullOrWhiteSpace(query.SearchTerm) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Playground
                        .Name
                        .ToLower()
                        .Contains(query.SearchTerm.ToLower()));
            }

            gamesQuery = query.Sorting switch
            {
                GameSorting.Town => gamesQuery.OrderBy(g => g.Playground.Town),
                GameSorting.PlaygroundName => gamesQuery.OrderBy(g => g.Playground.Name),
                GameSorting.DateCreated or _ => gamesQuery.OrderBy(g => g.Id)
            };

            var totalGames = gamesQuery.Count();

            var games = gamesQuery
                .Skip((query.CurrentPage - 1) * GameAllQueryModel.GamesPerPage)
                .Take(GameAllQueryModel.GamesPerPage)
                .Select(p => new GameListingViewModel()
                {
                    Id = p.Id,
                    Playground = p.Playground,
                    Date = p.Date,
                }).AsEnumerable();

            var towns = this.data
                .Playgrounds
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

            query.TotalGames = totalGames;
            query.Games = games;
            query.Towns = towns;

            return query;
        }
    }
}
