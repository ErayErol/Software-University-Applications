namespace MessiFinder.Services.Games
{
    using Data;
    using Data.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MessiFinder.Models;

    public class GameService : IGameService
    {
        private readonly MessiFinderDbContext data;
        private readonly IConfigurationProvider mapper;

        public GameService(MessiFinderDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public GameQueryServiceModel All(
            string town,
            string searchTerm,
            GameSorting sorting,
            int currentPage,
            int gamesPerPage)
        {
            var gamesQuery = this.data.Games.AsQueryable();

            if (string.IsNullOrWhiteSpace(town) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Playground.Town == town);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Playground
                        .Name
                        .ToLower()
                        .Contains(searchTerm.ToLower()));
            }

            gamesQuery = sorting switch
            {
                GameSorting.Town => gamesQuery.OrderBy(g => g.Playground.Town),
                GameSorting.PlaygroundName => gamesQuery.OrderBy(g => g.Playground.Name),
                GameSorting.DateCreated or _ => gamesQuery.OrderBy(g => g.Id)
            };

            var totalGames = gamesQuery.Count();

            var games = GetGames(gamesQuery
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage));

            return new GameQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalGames = totalGames,
                GamesPerPage = gamesPerPage,
                Games = games,
            };
        }

        public int Create(
            int playgroundId,
            string description,
            DateTime date,
            int numberOfPlayers,
            bool goalkeeper,
            bool ball,
            bool jerseys,
            int adminId)
        {
            var game = new Game
            {
                PlaygroundId = playgroundId,
                Description = description,
                Date = date,
                NumberOfPlayers = numberOfPlayers,
                Goalkeeper = goalkeeper,
                Ball = ball,
                Jerseys = jerseys,
                AdminId = adminId,
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();

            return game.Id;
        }

        public IEnumerable<GameListingServiceModel> ByUser(string userId)
            => GetGames(this.data
                .Games
                .Where(g => g.Admin.UserId == userId));

        public List<GameListingServiceModel> Latest()
            => this.data
                .Games
                .OrderByDescending(g => g.Id)
                .ProjectTo<GameListingServiceModel>(this.mapper)
                .Take(3)
                .ToList();

        public GameDetailsServiceModel Details(int id)
            => this.data
                .Games
                .Where(g => g.Id == id)
                .ProjectTo<GameDetailsServiceModel>(this.mapper)
                .FirstOrDefault();

        public bool IsByAdmin(int id, int adminId)
            => this.data
                .Games
                .Any(c => c.Id == id && c.AdminId == adminId);

        public bool Edit(
            int id,
            DateTime? date,
            int? numberOfPlayers,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description)
        {
            var game = this.data.Games.Find(id);

            if (game == null)
            {
                return false;
            }

            // TODO: maybe date and int have to be nullable

            game.Date = date.Value;
            game.NumberOfPlayers = numberOfPlayers.Value;
            game.Ball = ball;
            game.Jerseys = jerseys;
            game.Goalkeeper = goalkeeper;
            game.Description = description;

            this.data.SaveChanges();

            return true;
        }

        private static IEnumerable<GameListingServiceModel> GetGames(IQueryable<Game> gameQuery)
            => gameQuery
                .Select(g => new GameListingServiceModel
                {
                    Id = g.Id,
                    Playground = g.Playground,
                    Date = g.Date,
                })
                .ToList();
    }
}
