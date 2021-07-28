namespace MessiFinder.Services.Games
{
    using Data;
    using Data.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameService : IGameService
    {
        private readonly MessiFinderDbContext data;

        public GameService(MessiFinderDbContext data)
        {
            this.data = data;
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

            var games = gamesQuery
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage)
                .Select(g => new GameListingServiceModel
                {
                    Id = g.Id,
                    Playground = g.Playground,
                    Date = g.Date,
                });

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
    }
}
