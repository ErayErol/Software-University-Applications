namespace MiniFootball.Services.Games
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using MiniFootball.Models;

    public interface IGameService
    {
        GameQueryServiceModel All(
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int gamesPerPage);

        string Create(int fieldId,
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
            int adminId);

        IEnumerable<GameListingServiceModel> ByUser(string userId);
        
        IEnumerable<GameListingServiceModel> Latest();

        GameDetailsServiceModel GetDetails(string id);

        bool IsByAdmin(string id, int adminId);

        bool Edit(string id,
            DateTime? date,
            int? time,
            int? numberOfPlayers,
            string facebookUrl,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description);

        bool AddUserToGame(string id, string userId);
        
        bool IsUserIsJoinGame(string id, string userId);

        IQueryable<GameSeePlayersServiceModel> SeePlayers(string id);
        
        bool Delete(string id);
        
        GameIdUserIdServiceModel GameIdUserId(string id);

        bool IsExist(int fieldId, DateTime date, int time);
    }
}
