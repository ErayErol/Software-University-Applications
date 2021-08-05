namespace MiniFootball.Services.Games
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MiniFootball.Models;

    public interface IGameService
    {
        GameQueryServiceModel All(
            string town,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int gamesPerPage);

        string Create(
            int fieldId,
            string description,
            DateTime date,
            int numberOfPlayers,
            bool goalkeeper,
            bool ball,
            bool jerseys,
            int places,
            bool hasPlaces,
            int adminId);

        IEnumerable<GameListingServiceModel> ByUser(string userId);
        
        IEnumerable<GameListingServiceModel> Latest();

        GameDetailsServiceModel GetDetails(string id);

        bool IsByAdmin(string id, int adminId);

        bool Edit(
            string id,
            DateTime? date,
            int? numberOfPlayers,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description);

        bool AddUserToGame(string id, string userId);
        
        bool IsUserIsJoinGame(string id, string userId);

        IQueryable<string> SeePlayers(string id);
        
        bool Delete(string id);
    }
}
