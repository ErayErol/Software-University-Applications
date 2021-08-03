namespace MiniFootball.Services.Fields
{
    using Data;
    using Data.Models;
    using Games.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class FieldService : IFieldService
    {
        private readonly MiniFootballDbContext data;

        public FieldService(MiniFootballDbContext data)
        {
            this.data = data;
        }

        // TODO: In All Add button Info 
        public FieldQueryServiceModel All(
            string town,
            string searchTerm,
            GameSorting sorting,
            int currentPage,
            int fieldsPerPage)
        {
            var fieldsQuery = this.data.Fields.AsQueryable();

            if (string.IsNullOrWhiteSpace(town) == false)
            {
                fieldsQuery = fieldsQuery.Where(g => g.Town == town);
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
                GameSorting.Town => fieldsQuery.OrderBy(g => g.Town),
                GameSorting.FieldName => fieldsQuery.OrderBy(g => g.Name),
                GameSorting.DateCreated or _ => fieldsQuery.OrderBy(g => g.Id)
            };

            var totalPlaygrounds = fieldsQuery.Count();

            var fields = fieldsQuery
                .Skip((currentPage - 1) * fieldsPerPage)
                .Take(fieldsPerPage)
                .Select(p => new FieldServiceModel
                {
                    Town = p.Town,
                    Country = p.Country,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Address = p.Address,
                }).AsEnumerable();

            return new FieldQueryServiceModel
            {
                Fields = fields,
                TotalFields = totalPlaygrounds
            };
        }

        public int Create(
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
            string description)
        {
            var field = new Field
            {
                Name = name,
                Country = country,
                Town = town,
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

        public bool IsSame(string name, string country, string town, string address)
            => this.data
                .Fields
                .Any(p => p.Name == name &&
                          p.Country == country &&
                          p.Town == town &&
                          p.Address == address);

        public IEnumerable<string> Towns()
            => this.data
                .Fields
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

        public IEnumerable<FieldListingServiceModel> PlaygroundsListing(string town, string country)
            => this.data
                .Fields
                .Where(x => x.Town == town && x.Country == country)
                .Select(x => new FieldListingServiceModel
                {
                    FieldId = x.Id,
                    Name = x.Name,
                }).ToList();

        public bool PlaygroundExist(int fieldId)
            => this.data.Fields.Any(p => p.Id == fieldId);
    }
}
