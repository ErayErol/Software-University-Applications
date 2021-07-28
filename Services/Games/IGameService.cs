namespace MessiFinder.Services.Games
{
    using Models;
    using System;

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
    }
}
