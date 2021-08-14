namespace MiniFootball.Services.Games
{
    using MiniFootball.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IGameService
    {
        GameQueryServiceModel All(
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int gamesPerPage);

        string Create(
            int fieldId,
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
            string phoneNumber);

        bool Edit(
            string id,
            DateTime? date,
            int? time,
            int? numberOfPlayers,
            string facebookUrl,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description);

        IEnumerable<GameListingServiceModel> GamesWhereCreatorIsUser(string userId);

        IEnumerable<GameListingServiceModel> Latest();

        IQueryable<GameSeePlayersServiceModel> SeePlayers(string gameId);

        GameDetailsServiceModel GetDetails(string id);

        GameDeleteServiceModel GameDeleteInfo(string gameId);

        bool Delete(string gameId);

        bool IsAdminCreatorOfGame(string gameId, int adminId);

        bool AddUserToGame(string gameId, string userId);
        
        bool IsUserIsJoinGame(string gameId, string userId);
        
        bool IsFieldAlreadyReserved(int fieldId, DateTime date, int time);
        
        bool RemoveUserFromGame(string gameId, string userIdToDelete);
    }
}
