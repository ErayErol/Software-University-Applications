namespace MiniFootball.Services.Games
{
    using Data;
    using Data.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MiniFootball.Models;

    public class GameService : IGameService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;

        public GameService(MiniFootballDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        // TODO: Maybe without image is better and add count of free places
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
                    .Where(g => g.Field.Town == town);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Field
                        .Name
                        .ToLower()
                        .Contains(searchTerm.ToLower()));
            }

            gamesQuery = sorting switch
            {
                GameSorting.Town => gamesQuery.OrderBy(g => g.Field.Town),
                GameSorting.FieldName => gamesQuery.OrderBy(g => g.Field.Name),
                GameSorting.DateCreated or _ => gamesQuery.OrderByDescending(g => g.Id)
            };

            var totalGames = gamesQuery.Count();

            var games = GetGames(gamesQuery
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage), this.mapper);

            return new GameQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalGames = totalGames,
                GamesPerPage = gamesPerPage,
                Games = games,
            };
        }

        public string Create(
            int fieldId,
            string description,
            DateTime date,
            int numberOfPlayers,
            bool goalkeeper,
            bool ball,
            bool jerseys,
            int places,
            bool hasPlaces,
            int adminId)
        {
            // TODO: Only admins can create (not managers, not users)
            var game = new Game
            {
                FieldId = fieldId,
                Description = description,
                Date = date,
                NumberOfPlayers = numberOfPlayers,
                Goalkeeper = goalkeeper,
                Ball = ball,
                Jerseys = jerseys,
                Places = places,
                HasPlaces = hasPlaces,
                AdminId = adminId,
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();

            return game.Id;
        }

        public bool Edit(
            string id,
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

        public bool AddUserToGame(string id, string userId)
        {
            // TODO: If all people are joined then replace JOIN button with No needed player or something like this..

            var game = this.data
                .Games
                .FirstOrDefault(c => c.Id == id);

            if (game == null)
            {
                return false;
            }

            if (game.HasPlaces)
            {
                game.Places--;
            }

            var userGame = new UserGame
            {
                GameId = game.Id,
                UserId = userId
            };

            this.data.UserGames.Add(userGame);

            this.data.SaveChanges();

            return true;
        }

        public bool IsUserIsJoinGame(string id, string userId)
            => this.data.UserGames
                .Any(c => c.GameId == id && c.UserId == userId);

        public IQueryable<string> SeePlayers(string id)
        => this.data
                .UserGames
                .Where(g => g.GameId == id)
                .Select(u => u.User.UserName);


        public IEnumerable<GameListingServiceModel> ByUser(string userId)
            => GetGames(
                this.data
                    .Games
                    .Where(g => g.Admin.UserId == userId),
                this.mapper);

        public IEnumerable<GameListingServiceModel> Latest()
            => this.data
                .Games
                .OrderByDescending(g => g.Id)
                .ProjectTo<GameListingServiceModel>(this.mapper)
                .Take(3)
                .ToList();

        public GameDetailsServiceModel GetDetails(string id)
            => this.data
                .Games
                .Where(g => g.Id == id)
                .ProjectTo<GameDetailsServiceModel>(this.mapper)
                .FirstOrDefault();

        public bool IsByAdmin(string id, int adminId)
            => this.data
                .Games
                .Any(c => c.Id == id && c.AdminId == adminId);

        private static IEnumerable<GameListingServiceModel> GetGames(
            IQueryable<Game> gameQuery,
            IConfigurationProvider mapper)
                => gameQuery
                    .ProjectTo<GameListingServiceModel>(mapper)
                    .ToList();
    }
}
