namespace MiniFootball.Services.Games
{
    using MiniFootball.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IGameService
    {
        GameQueryServiceModel All(string cityName = null,
                                  string searchTerm = null,
                                  Sorting sorting = Sorting.DateCreated,
                                  int currentPage = 1,
                                  int gamesPerPage = int.MaxValue,
                                  bool publicOnly = true);

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
                      int adminId, 
                      string phoneNumber);

        bool Edit(string id,
                  DateTime? date,
                  int? time,
                  int? numberOfPlayers,
                  string facebookUrl,
                  bool ball,
                  bool jerseys,
                  bool goalkeeper,
                  string description,
                  bool isPublic);

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
        
        void ChangeVisibility(string id);
    }
}
