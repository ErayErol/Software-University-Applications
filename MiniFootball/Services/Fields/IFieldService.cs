namespace MiniFootball.Services.Fields
{
    using Games.Models;
    using Models;
    using System.Collections.Generic;

    public interface IFieldService
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

        IEnumerable<FieldListingServiceModel> PlaygroundsListing(string town, string country);

        bool PlaygroundExist(int playgroundId);

        FieldQueryServiceModel All(
            string town,
            string searchTerm,
            GameSorting sorting,
            int currentPage,
            int playgroundsPerPage);
    }
}
