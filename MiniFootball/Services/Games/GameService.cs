namespace MiniFootball.Services.Games
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using MiniFootball.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameService : IGameService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;

        public GameService(
            MiniFootballDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public GameQueryServiceModel All(
            string cityName = null,
            string searchTerm = null,
            Sorting sorting = Sorting.DateCreated,
            int currentPage = 1,
            int gamesPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var city = data
                .Cities
                .FirstOrDefault(c => c.Name == cityName);

            var gamesQuery = data.Games
                .Where(g => !publicOnly || g.IsPublic);

            if (string.IsNullOrWhiteSpace(city?.Name) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Field.City.Name == city.Name);
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
                Sorting.City
                    => gamesQuery
                        .OrderBy(g => g.Field.City),
                Sorting.FieldName
                    => gamesQuery
                        .OrderBy(g => g.Field.Name),
                Sorting.DateCreated or _
                    => gamesQuery
                        .OrderByDescending(g => g.Date.Date)
            };

            var totalGames = gamesQuery.Count();

            var games = GetGames(gamesQuery
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage), mapper)
                .ToList();

            foreach (var gameListingServiceModel in games)
            {
                gameListingServiceModel.Field.Country = data.Countries.Find(gameListingServiceModel.Field.CountryId);
                gameListingServiceModel.Field.City = data.Cities.Find(gameListingServiceModel.Field.CityId);
            }

            return new GameQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalGames = totalGames,
                GamesPerPage = gamesPerPage,
                Games = games,
            };
        }

        public string Create(int fieldId,
            DateTime date,
            int time,
            int numberOfPlayers,
            string facebookUrl,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description,
            int places,
            bool hasPlaces,
            int adminId, 
            string phoneNumber)
        {
            var game = new Game
            {
                FieldId = fieldId,
                Date = date,
                Time = time,
                NumberOfPlayers = numberOfPlayers,
                FacebookUrl = facebookUrl,
                Ball = ball,
                Jerseys = jerseys,
                Goalkeeper = goalkeeper,
                Description = description,
                Places = places,
                HasPlaces = hasPlaces,
                AdminId = adminId,
                PhoneNumber = phoneNumber,
                IsPublic = false,
            };

            data.Games.Add(game);
            data.SaveChanges();

            return game.Id;
        }

        public bool Edit(
            string id,
            DateTime? date,
            int? time,
            int? numberOfPlayers,
            string facebookUrl,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description,
            bool isPublic)
        {
            var game = data.Games.Find(id);

            if (game == null)
            {
                return false;
            }

            if (game.NumberOfPlayers != numberOfPlayers.Value)
            {
                game.Places = numberOfPlayers.Value;

                var joinedPlayers = SeePlayers(game.Id).Count();
                game.Places -= joinedPlayers;
            }

            game.Date = date.Value;
            game.Time = time.Value;
            game.NumberOfPlayers = numberOfPlayers.Value;
            game.FacebookUrl = facebookUrl;
            game.Ball = ball;
            game.Jerseys = jerseys;
            game.Goalkeeper = goalkeeper;
            game.Description = description;
            game.IsPublic = isPublic;

            data.SaveChanges();

            return true;
        }

        public bool AddUserToGame(string gameId, string userId)
        {
            var game = data
                .Games
                .FirstOrDefault(c => c.Id == gameId);

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

            data.UserGames.Add(userGame);
            data.SaveChanges();

            return true;
        }

        public bool IsUserIsJoinGame(string gameId, string userId)
            => data.UserGames
                .Any(c => c.GameId == gameId && c.UserId == userId);

        public IQueryable<GameSeePlayersServiceModel> SeePlayers(string gameId)
        {
            var games =  data
                .UserGames
                .Where(g => g.GameId == gameId)
                .Select(g => new GameSeePlayersServiceModel
                {
                    GameId = gameId,
                    UserId = g.User.Id,
                    ImageUrl = g.User.ImageUrl,
                    FirstName = g.User.FirstName,
                    LastName = g.User.LastName,
                    NickName = g.User.NickName,
                    PhoneNumber = g.User.PhoneNumber,
                    IsCreator = GameDeleteInfo(gameId).UserId == g.UserId,
                });

            return games;
        }

        public bool Delete(string gameId)
        {
            var game = data
                .Games
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return false;
            }

            data.Remove(game);
            data.SaveChanges();

            return true;
        }

        public GameDeleteServiceModel GameDeleteInfo(string gameId)
            => data
                .Games
                .Where(g => g.Id == gameId)
                .ProjectTo<GameDeleteServiceModel>(mapper)
                .FirstOrDefault();

        public bool IsFieldAlreadyReserved(int fieldId, DateTime date, int time) 
            => data.Games
                .Any(g => g.FieldId.Equals(fieldId) && g.Date.Equals(date) && g.Time.Equals(time));

        public bool RemoveUserFromGame(string gameId, string userIdToDelete)
        {
            var userGame = data
                .UserGames
                .FirstOrDefault(ug => ug.GameId == gameId && ug.UserId == userIdToDelete);

            if (userGame == null)
            {
                return false;
            }

            data.UserGames.Remove(userGame);
            data.SaveChanges();

            return true;
        }

        public IEnumerable<GameListingServiceModel> GamesWhereCreatorIsUser(string userId)
        {
            var games = GetGames(
                data
                    .Games
                    .Where(g => g.Admin.UserId == userId),
                mapper)
                .ToList();

            foreach (var gameListingServiceModel in games)
            {
                gameListingServiceModel.Field.Country = data.Countries.Find(gameListingServiceModel.Field.CountryId);
                gameListingServiceModel.Field.City = data.Cities.Find(gameListingServiceModel.Field.CityId);
            }

            return games;
        }

        public GameDetailsServiceModel GetDetails(string id)
        {
            var gameDetails = data
                .Games
                .Where(g => g.Id == id)
                .ProjectTo<GameDetailsServiceModel>(mapper)
                .FirstOrDefault();

            if (gameDetails == null)
            {
                return null;
            }

            var joinedPayers = SeePlayers(id).Count();
            gameDetails.JoinedPlayersCount = joinedPayers;

            return gameDetails;
        }

        public void ChangeVisibility(string id)
        {
            var game = data.Games.Find(id);

            game.IsPublic = !game.IsPublic;

            data.SaveChanges();
        }

        public IEnumerable<GameListingServiceModel> Latest()
            => data
                .Games
                .Where(g => g.IsPublic)
                .OrderByDescending(g => g.Date.Date)
                .ProjectTo<GameListingServiceModel>(mapper)
                .Take(3)
                .ToList();

        public bool IsAdminCreatorOfGame(string gameId, int adminId)
            => data
                .Games
                .Any(c => c.Id == gameId && c.AdminId == adminId);

        private static IEnumerable<GameListingServiceModel> GetGames(
            IQueryable<Game> gameQuery,
            IConfigurationProvider mapper)
                => gameQuery
                    .ProjectTo<GameListingServiceModel>(mapper)
                    .ToList();
    }
}
