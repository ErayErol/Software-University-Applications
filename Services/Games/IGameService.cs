namespace MessiFinder.Services.Games
{
    using System;
    using System.Collections.Generic;
    using Models;

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

        IEnumerable<string> Towns();

        IEnumerable<PlaygroundListingServiceModel> PlaygroundsListing(string town, string country);

        bool PlaygroundExist(int playgroundId);
    }
}
