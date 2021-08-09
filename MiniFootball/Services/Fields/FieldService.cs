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
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int fieldsPerPage)
        {
            var fieldsQuery = this.data.Fields.AsQueryable();

            var city = this.data
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
                .Take(fieldsPerPage), this.mapper)
                .ToList();

            return new FieldQueryServiceModel
            {
                Fields = fields,
                TotalFields = totalPlaygrounds
            };
        }

        public bool IsCorrectCountryAndCity(int fieldId, string name, string countryName, string cityName)
        {
            var field = this.data.Fields.FirstOrDefault(f => f.Id == fieldId);

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
            string description)
        {
            var country = this.data
                .Countries
                .FirstOrDefault(c => c.Id == countryId);

            var city = this.data
                .Cities
                .FirstOrDefault(c => c.Id == cityId);

            var field = new Field
            {
                Name = name,
                Country = country,
                City = city,
                Address = address,
                ImageUrl = imageUrl,
                PhoneNumber = phoneNumber,
                Parking = parking,
                Cafe = cafe,
                Shower = shower,
                ChangingRoom = changingRoom,
                Description = description,
            };

            this.data.Fields.Add(field);
            this.data.SaveChanges();

            return field.Id;
        }

        public bool IsSame(string name, int countryId, int cityId)
            => this.data
                .Fields
                .Any(p => p.Name == name && p.Country.Id == countryId && p.City.Id == cityId);

        public IEnumerable<string> Cities()
            => this.data
                .Fields
                .Select(p => p.City.Name)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

        public IEnumerable<FieldListingServiceModel> FieldsListing(string cityName, string countryName)
            => this.data
                .Fields
                .Where(x => x.City.Name == cityName && x.Country.Name == countryName)
                .ProjectTo<FieldListingServiceModel>(mapper)
                .ToList();

        public bool FieldExist(int fieldId)
            => this.data
                .Fields
                .Any(p => p.Id == fieldId);

        public string FieldName(int fieldId)
            => this.data
                .Fields
                .Where(f => f.Id == fieldId)
                .Select(f => f.Name)
                .FirstOrDefault();

        private static IEnumerable<FieldServiceModel> GetFields(
            IQueryable<Field> fieldQuery,
            IConfigurationProvider mapper)
            => fieldQuery
                .ProjectTo<FieldServiceModel>(mapper)
                .ToList();
    }
}
