namespace MiniFootball.Services.Fields
{
    using Models;
    using System.Collections.Generic;
    using Games.Models;

    public interface IFieldService
    {
        FieldQueryServiceModel All(
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int fieldsPerPage);

        int Create(string name,
            int countryId,
            int cityId,
            string address,
            string imageUrl,
            string phoneNumber,
            bool parking,
            bool cafe,
            bool shower,
            bool changingRoom,
            string description, int adminId);

        bool Edit(int id,
            string name,
            string address,
            string imageUrl,
            bool parking,
            bool shower,
            bool changingRoom,
            bool cafe,
            string description, string phoneNumber);

        bool IsAlreadyExist(
            string name,
            int countryId,
            int cityId);

        IEnumerable<string> AllCreatedCitiesName();

        IEnumerable<GameFieldListingServiceModel> FieldsListing(string cityName, string countryName);

        IEnumerable<FieldListingServiceModel> FieldsWhereCreatorIsUser(string id);

        FieldDetailServiceModel GetDetails(int id);

        string FieldName(int fieldId);

        bool FieldExist(int fieldId);

        bool Delete(int id);
        
        bool IsAdminCreatorOfField(int id, int adminId);

        bool IsCorrectParameters(int fieldId, string name, string country, string city);
    }
}
