namespace MessiFinder.Services.Games
{
    using Models;
    using System;
    using System.Collections.Generic;

    public interface IGameService
    {
        GameQueryServiceModel All(
            string town,
            string searchTerm,
            GameSorting sorting,
            int currentPage,
            int gamesPerPage);

        int Create(
            int playgroundId,
            string description,
            DateTime date,
            int numberOfPlayers,
            bool goalkeeper,
            bool ball,
            bool jerseys,
            int adminId);

        IEnumerable<GameListingServiceModel> ByUser(string userId);

        GameDetailsServiceModel Details(int id);

        bool IsByAdmin(int id, int adminId);

        bool Edit(
            int id,
            DateTime? date,
            int? numberOfPlayers,
            bool ball,
            bool jerseys,
            bool goalkeeper,
            string description);
    }
}
