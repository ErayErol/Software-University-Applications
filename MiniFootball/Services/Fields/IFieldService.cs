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

        IEnumerable<FieldListingServiceModel> FieldsListing(string town, string country);

        bool FieldExist(int fieldId);

        FieldQueryServiceModel All(
            string town,
            string searchTerm,
            GameSorting sorting,
            int currentPage,
            int fieldsPerPage);

        bool IsCorrectCountryAndTown(int fieldId, string name, string country, string town);

        string FieldName(int fieldId);
    }
}
