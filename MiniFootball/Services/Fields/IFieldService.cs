namespace MiniFootball.Services.Fields
{
    using System;
    using Games.Models;
    using Models;
    using System.Collections.Generic;

    public interface IFieldService
    {
        int Create(
            string name,
            int countryId,
            int cityId,
            string address,
            string imageUrl,
            string phoneNumber,
            bool parking,
            bool cafe,
            bool shower,
            bool changingRoom,
            string description);

        bool IsExist(
            string name,
            int countryId,
            int cityId);

        IEnumerable<string> Cities();

        IEnumerable<FieldListingServiceModel> FieldsListing(string cityName, string countryName);

        bool FieldExist(int fieldId);

        FieldQueryServiceModel All(
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int fieldsPerPage);

        bool IsCorrectCountryAndCity(int fieldId, string name, string country, string city);

        string FieldName(int fieldId);
        
        FieldDetailServiceModel GetDetails(int id);
    }
}
