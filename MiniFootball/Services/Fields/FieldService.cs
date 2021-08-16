namespace MiniFootball.Services.Fields
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Games.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using static Convert;

    public class FieldService : IFieldService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;

        public FieldService(
            MiniFootballDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public FieldQueryServiceModel All(
            string cityName = null,
            string searchTerm = null,
            Sorting sorting = Sorting.DateCreated,
            int currentPage = 1,
            int fieldsPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var fieldsQuery = data.Fields
                .Where(f => !publicOnly || f.IsPublic);

            var city = data
                .Cities
                .FirstOrDefault(c => c.Name == cityName);

            if (string.IsNullOrWhiteSpace(city?.Name) == false)
            {
                fieldsQuery = fieldsQuery
                    .Where(g => g.City.Name == city.Name);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                fieldsQuery = fieldsQuery
                    .Where(g => g
                        .Name
                        .ToLower()
                        .Contains(searchTerm.ToLower()));
            }

            fieldsQuery = sorting switch
            {
                Sorting.City
                    => fieldsQuery
                        .OrderBy(g => g.City.Name),
                Sorting.FieldName
                    => fieldsQuery
                        .OrderBy(g => g.Name),
                Sorting.DateCreated or _
                    => fieldsQuery
                        .OrderBy(g => g.Id)
            };

            var totalPlaygrounds = fieldsQuery.Count();

            var fields = GetFields(fieldsQuery
                .Skip((currentPage - 1) * fieldsPerPage)
                .Take(fieldsPerPage), mapper)
                .ToList();

            return new FieldQueryServiceModel
            {
                Fields = fields,
                TotalFields = totalPlaygrounds
            };
        }

        public int Create(
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
            string description,
            int adminId)
        {
            var field = new Field
            {
                Name = ToTitleCase(name),
                CountryId = countryId,
                CityId = cityId,
                Address = ToSentenceCase(address),
                ImageUrl = imageUrl,
                PhoneNumber = phoneNumber,
                Parking = parking,
                Cafe = cafe,
                Shower = shower,
                ChangingRoom = changingRoom,
                Description = ToSentenceCase(description),
                AdminId = adminId,
                IsPublic = false,
            };

            data.Fields.Add(field);
            data.SaveChanges();

            return field.Id;
        }

        public bool Edit(
            int id,
            string name,
            string address,
            string imageUrl,
            bool parking,
            bool shower,
            bool changingRoom,
            bool cafe,
            string description,
            string phoneNumber,
            bool isPublic)
        {
            var field = data.Fields.Find(id);

            if (field == null)
            {
                return false;
            }

            field.PhoneNumber = phoneNumber;
            field.Name = ToTitleCase(name);
            field.Address = ToSentenceCase(address);
            field.ImageUrl = imageUrl;
            field.Parking = parking;
            field.Shower = shower;
            field.ChangingRoom = changingRoom;
            field.Cafe = cafe;
            field.Description = ToSentenceCase(description);
            field.IsPublic = isPublic;

            data.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var field = data.Fields.Find(id);

            if (field == null)
            {
                return false;
            }

            var gamesToRemove = data
                .Games
                .Where(g => g.FieldId == field.Id);

            data.Games.RemoveRange(gamesToRemove);
            data.SaveChanges();

            data.Fields.Remove(field);
            data.SaveChanges();

            return true;
        }

        public bool IsCorrectParameters(int fieldId, string name, string countryName, string cityName)
        {
            var field = data.Fields.FirstOrDefault(f => f.Id == fieldId);

            if (field == null)
            {
                return false;
            }

            field.Country = data.Countries.Find(field.CountryId);
            field.City = data.Cities.Find(field.CityId);

            return
                field != null &&
                field.Name.ToLower() == name.ToLower() &&
                field.Country.Name.ToLower() == countryName.ToLower() &&
                field.City.Name.ToLower() == cityName.ToLower();
        }

        public void ChangeVisibility(int id)
        {
            var field = this.data.Fields.Find(id);

            field.IsPublic = !field.IsPublic;

            this.data.SaveChanges();
        }

        public bool IsAlreadyExist(string name, int countryId, int cityId)
            => data
                .Fields
                .Any(p => p.Name == name && p.Country.Id == countryId && p.City.Id == cityId);

        public IEnumerable<string> AllCreatedCitiesName()
            => data
                .Fields
                .Select(p => p.City.Name)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

        public IEnumerable<GameFieldListingServiceModel> FieldsListing(string cityName, string countryName)
            => data
                .Fields
                .Where(x => x.City.Name == cityName && x.Country.Name == countryName)
                .ProjectTo<GameFieldListingServiceModel>(mapper)
                .ToList();

        public bool FieldExist(int fieldId)
            => data
                .Fields
                .Any(p => p.Id == fieldId);

        public FieldDeleteServiceModel FieldDeleteInfo(int id)
            => data
                .Fields
                .Where(f => f.Id == id)
                .ProjectTo<FieldDeleteServiceModel>(mapper)
                .FirstOrDefault();

        public string FieldName(int fieldId)
            => data
                .Fields
                .Where(f => f.Id == fieldId)
                .Select(f => f.Name)
                .FirstOrDefault();

        public FieldDetailServiceModel GetDetails(int id)
            => data
                .Fields
                .Where(f => f.Id == id)
                .ProjectTo<FieldDetailServiceModel>(mapper)
                .FirstOrDefault();

        public IEnumerable<FieldListingServiceModel> FieldsWhereCreatorIsUser(string userId)
            => GetFields(
                data
                    .Fields
                    .Where(g => g.Admin.UserId == userId),
                mapper);

        public bool IsAdminCreatorOfField(int id, int adminId)
            => data
                .Fields
                .Any(c => c.Id == id && c.AdminId == adminId);

        private static IEnumerable<FieldListingServiceModel> GetFields(
            IQueryable<Field> fieldQuery,
            IConfigurationProvider mapper)
            => fieldQuery
                .ProjectTo<FieldListingServiceModel>(mapper)
                .ToList();
    }
}
