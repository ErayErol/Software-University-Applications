namespace MessiFinder.Services.Playgrounds
{
    using System.Collections.Generic;
    using Games;
    using Games.Models;
    using Models;

    public interface IPlaygroundService
    {
        int Create(
            string name,
            string country,
            string town,
            string address,
            string imageUrl,
            string phoneNumber,
            bool parking,
            bool cafe,
            bool shower,
            bool changingRoom,
            string description);

        bool IsSame(
            string name,
            string country,
            string town,
            string address);

        IEnumerable<string> Towns();

        IEnumerable<PlaygroundListingServiceModel> PlaygroundsListing(string town, string country);

        bool PlaygroundExist(int playgroundId);

        PlaygroundQueryServiceModel All(
            string town,
            string searchTerm,
            GameSorting sorting,
            int currentPage,
            int playgroundsPerPage);
    }
}
