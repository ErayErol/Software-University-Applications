namespace MiniFootball.Test.Controllers
{
    using Data;
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Fields;
    using MyTested.AspNetCore.Mvc;
    using Services.Fields;
    using System.Linq;
    using Areas.Admin.Controllers;
    using Xunit;
    using static GlobalConstant;
    using static WebConstants;
    using Admin = MiniFootball.Data.Models.Admin;
    using City = MiniFootball.Data.Models.City;
    using Field = MiniFootball.Data.Models.Field;

    public class FieldsControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.All(new FieldAllQueryModel()))
                .ShouldReturn()
                .View();

        [Fact]
        public void GetCreateWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void GetCreateWhenUserIsAdminShouldReturnView()
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateWhenCityDoesNotExistShouldRedirectToCreateCities()
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.Create(new FieldCreateFormModel
                {
                    Name = "Avenue",
                    CountryId = 24,
                    CityId = 1,
                    Description = "In the summer this place is number 1 to play mini football.",
                    Address = "ул. Дунав 1 - в парка под супермаркет авеню",
                    ImageUrl = "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg",
                    Cafe = true,
                    ChangingRoom = true,
                    Parking = true,
                    Shower = true,
                    PhoneNumber = "0888888889",
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CitiesController>(c => c.Create()));

        [Theory]
        [InlineData(
            "Avenue",
            "Bulgaria",
            "Haskovo",
            "In the summer this place is number 1 to play mini football.",
            "Ул. дунав 1 - в парка под супермаркет авеню",
            "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg",
            true,
            true,
            true,
            true,
            "0888888889",
            1,
            1,
            1,
            1)]
        public void PostCreateWhenEverythingIsOkRedirectToMine(
            string name,
            string countryName,
            string cityName,
            string description,
            string address,
            string imageUrl,
            bool cafe,
            bool changingRoom,
            bool shower,
            bool parking,
            string phoneNumber,
            int fieldId,
            int countryId,
            int cityId,
            int adminId)
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.Create(new FieldCreateFormModel
                {
                    Name = name,
                    CountryName = countryName,
                    CityName = cityName,
                    Description = description,
                    Address = address,
                    ImageUrl = imageUrl,
                    Cafe = cafe,
                    ChangingRoom = changingRoom,
                    Parking = parking,
                    Shower = shower,
                    PhoneNumber = phoneNumber,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Field>(field => field
                        .Any(f =>
                            f.Id == fieldId &&
                            f.Name == name &&
                            f.CountryId == countryId &&
                            f.CityId == cityId &&
                            f.Description == description &&
                            f.Address == address &&
                            f.ImageUrl == imageUrl &&
                            f.Cafe == cafe &&
                            f.ChangingRoom == changingRoom &&
                            f.Parking == parking &&
                            f.Shower == shower &&
                            f.AdminId == adminId &&
                            f.PhoneNumber == phoneNumber)))
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<FieldsController>(c => c.Mine()));

        [Theory]
        [InlineData(1, "Avenue")]
        public void GetEditWhenUserIsNotAdminShouldRedirectToBecomeAdmin(int id, string name)
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Edit(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData(1, "Avenue")]
        public void GetEditWhenEverythingIsOkShouldReturnView(int id, string name)
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Field>(field => field
                            .Add(Fields.Avenue()))))
                .Calling(c => c.Edit(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostEditWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Edit(new FieldDetailServiceModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData(
            "Avenue",
            "In the summer this place is number 1 to play mini football.",
            "Ул. дунав 1 - в парка под супермаркет авеню",
            "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg",
            true,
            true,
            false,
            true,
            "0888888889",
            1,
            1,
            1,
            1)]
        public void PostEditWhenEverythingIsOkShouldReturnView(
            string name,
            string description,
            string address,
            string imageUrl,
            bool cafe,
            bool changingRoom,
            bool shower,
            bool parking,
            string phoneNumber,
            int fieldId,
            int adminId,
            int countryId,
            int cityId)
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Field>(field => field
                            .Add(Fields.Avenue()))))
                .Calling(c => c.Edit(new FieldDetailServiceModel
                {
                    Id = fieldId,
                    Name = name,
                    Description = description,
                    Address = address,
                    ImageUrl = imageUrl,
                    Cafe = cafe,
                    ChangingRoom = changingRoom,
                    Parking = parking,
                    Shower = shower,
                    PhoneNumber = phoneNumber,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Field>(field => field
                        .Any(f =>
                            f.Id == fieldId &&
                            f.Name == name &&
                            f.CountryId == countryId &&
                            f.CityId == cityId &&
                            f.Description == description &&
                            f.Address == address &&
                            f.ImageUrl == imageUrl &&
                            f.Cafe == cafe &&
                            f.ChangingRoom == changingRoom &&
                            f.Parking == parking &&
                            f.Shower == shower &&
                            f.AdminId == adminId &&
                            f.PhoneNumber == phoneNumber)))
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<FieldsController>(c => c.Details(fieldId, name)));

        [Theory]
        [InlineData(1, "Avenue")]
        public void GetDeleteWhenUserIsNotAdminShouldRedirectToBecomeAdmin(int id, string name)
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Delete(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData(1, "Avenue")]
        public void GetDeleteWhenEverythingIsOkShouldReturnView(int id, string name)
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Field>(field => field
                            .Add(Fields.Avenue()))))
                .Calling(c => c.Delete(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostDeleteWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Delete(new FieldDeleteServiceModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData(
            "Avenue",
            "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg",
            1,
            1,
            1,
            1)]
        public void PostDeleteWhenEverythingIsOkShouldReturnView(
            string name,
            string imageUrl,
            int fieldId,
            int adminId,
            int countryId,
            int cityId)
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Field>(field => field
                            .Add(Fields.Avenue()))))
                .Calling(c => c.Delete(new FieldDeleteServiceModel()
                {
                    Id = fieldId,
                    Name = name,
                    ImageUrl = imageUrl,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Field>(field => field
                        .Any(f =>
                            f.Id == fieldId &&
                            f.Name == name &&
                            f.CountryId == countryId &&
                            f.CityId == cityId &&
                            f.AdminId == adminId &&
                            f.ImageUrl == imageUrl) == false))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<FieldsController>(c => c.All(With.Any<FieldAllQueryModel>())));

        [Theory]
        [InlineData(1, "Avenue")]
        public void GetDetailsWhenIdIsIncorrectShouldReturnBadRequest(int id, string name)
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Details(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Home.Error, Home.ControllerName);

        [Theory]
        [InlineData(1, "Avenue")]
        public void GetDetailsWhenEverythingIsOkShouldReturnView(int id, string name)
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Field>(field => field
                            .Add(Fields.Avenue()))))
                .Calling(c => c.Details(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void GetMineWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<FieldsController>
                .Instance()
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void GetMineWhenEverythingIsOkShouldReturnView()
            => MyController<FieldsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Field>(field => field
                            .Add(Fields.Avenue()))))
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
