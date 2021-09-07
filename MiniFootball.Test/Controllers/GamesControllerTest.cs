namespace MiniFootball.Test.Controllers
{
    using Areas.Admin.Controllers;
    using Data;
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Games;
    using MyTested.AspNetCore.Mvc;
    using Services.Games.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static GlobalConstant;
    using static GlobalConstant.Game;
    using static WebConstants;
    using Admin = MiniFootball.Data.Models.Admin;
    using City = MiniFootball.Data.Models.City;
    using Field = MiniFootball.Data.Models.Field;
    using Game = MiniFootball.Data.Models.Game;

    public class GamesControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.All(new GameAllQueryModel()))
                .ShouldReturn()
                .View();

        [Fact]
        public void GetCreateGameFirstStepWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.CreateGameFirstStep())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void GetCreateGameFirstStepWhenUserIsAdminShouldReturnView()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.CreateGameFirstStep())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateGameFirstStepWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.CreateGameFirstStep(new CreateGameFirstStepViewModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData(
            "Bulgaria",
            "Haskovo")]
        public void PostCreateGameFirstStepWhenCityDoesNotExistShouldRedirectToCreateCity(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.CreateGameFirstStep(new CreateGameFirstStepViewModel
                {
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Create, GlobalConstant.City.ControllerName);

        [Theory]
        [InlineData(
            null,
            null)]
        public void PostCreateGameFirstStepWhenModelStateIsInvalidShouldReturnView(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.CreateGameFirstStep(new CreateGameFirstStepViewModel
                {
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(
            "Bulgaria",
            "Haskovo")]
        public void PostCreateGameFirstStepWhenEverythingIsOkShouldRedirectToCreateGameChooseField(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameFirstStep(new CreateGameFirstStepViewModel
                {
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(
                    CreateGameChooseField,
                    ControllerName,
                    new CreateGameCountryAndCityViewModel
                    {
                        CountryName = countryName,
                        CityName = cityName,
                    });

        [Fact]
        public void GetCreateGameChooseFieldWhenUserIsNotAdminShouldReturnView()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.CreateGameChooseField(new CreateGameCountryAndCityViewModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(
            "Bulgaria",
            "Haskovo")]
        public void GetCreateGameChooseFieldWhenUserIsAdminShouldReturnView(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameChooseField(new CreateGameCountryAndCityViewModel
                {
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(new CreateGameSecondStepViewModel
                {
                    Fields = new List<GameFieldListingServiceModel>(),
                    CountryName = countryName,
                    CityName = cityName,
                });

        [Fact]
        public void PostCreateGameChooseFieldWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.CreateGameChooseField(new CreateGameSecondStepViewModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData(
            "Bulgaria",
            "Haskovo")]
        public void PostCreateGameChooseFieldWhenFieldDoesNotExistShouldRedirectToHomeError(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameChooseField(new CreateGameSecondStepViewModel
                {
                    FieldId = 1,
                    Name = "Avenue",
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Home.Error, Home.ControllerName);

        [Theory]
        [InlineData(
            "Bulgaria",
            "Haskovo")]
        public void PostCreateGameChooseFieldWhenEverythingIsOkShouldRedirectToCreateGameLastStep(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameChooseField(new CreateGameSecondStepViewModel
                {
                    FieldId = 1,
                    Name = "Avenue",
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(
                    CreateGameLastStep,
                    ControllerName,
                    new CreateGameLastStepViewModel
                    {
                        FieldId = 1,
                        Name = "Avenue",
                        CountryName = countryName,
                        CityName = cityName,
                    });

        [Fact]
        public void GetCreateGameLastStepWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.CreateGameLastStep(new CreateGameLastStepViewModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void GetCreateGameLastStepWhenFieldParametersAreIncorrectShouldRedirectToHomeError()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.CreateGameLastStep(new CreateGameLastStepViewModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Home.Error, Home.ControllerName);

        [Theory]
        [InlineData(
            "Bulgaria",
            "Haskovo")]
        public void GetCreateGameLastStepWhenEverythingIsOkShouldReturnView(
            string countryName,
            string cityName)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameLastStep(new CreateGameLastStepViewModel
                {
                    FieldId = 1,
                    Name = "Avenue",
                    CountryName = countryName,
                    CityName = cityName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateGameLastStepWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.CreateGameLastStep(new CreateGameFormModel
                {
                    NumberOfPlayers = 12,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void PostCreateGameLastStepWhenModelStateIsInvalidShouldReturnView()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.CreateGameLastStep(new CreateGameFormModel
                {
                    NumberOfPlayers = 12,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateGameLastStepWhenDateIsCorrectShouldReturnView()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameLastStep(new CreateGameFormModel
                {
                    Time = 20,
                    NumberOfPlayers = 12,
                    Date = DateTime.Now.Date.AddDays(-1),
                    Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    FieldId = 1,
                    Ball = true,
                    FacebookUrl = "https://www.youtube.com/watch?v=5c5m8Su7l1c&list=PL9s7A_reW2Si6zlpekLWSayvHP0-Buh62&index=24&t=5389s",
                    FieldPhotoPath = "https://static3.depositphotos.com/1003596/247/i/950/depositphotos_2477946-stock-photo-erotic-sexy-girl-icon-eblem.jpg",
                    FieldName = "Avenue",
                    Goalkeeper = false,
                    Jerseys = true,

                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateGameLastStepWhenIsFieldAlreadyReservedShouldReturnView()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser((TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Game>(game => game
                            .Add(Games.NewGame()))))
                .Calling(c => c.CreateGameLastStep(new CreateGameFormModel
                {
                    Time = 20,
                    NumberOfPlayers = 12,
                    Date = DateTime.Now.Date.AddDays(1),
                    Description = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    FieldId = 1,
                    Ball = true,
                    FacebookUrl = "https://www.youtube.com/watch?v=5c5m8Su7l1c&list=PL9s7A_reW2Si6zlpekLWSayvHP0-Buh62&index=24&t=5389s",
                    FieldPhotoPath = "https://static3.depositphotos.com/1003596/247/i/950/depositphotos_2477946-stock-photo-erotic-sexy-girl-icon-eblem.jpg",
                    FieldName = "Avenue",
                    Goalkeeper = false,
                    Jerseys = true,

                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(
            20,
            12,
            "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            1,
            true,
            "https://www.youtube.com/watch?v=5c5m8Su7l1c&list=PL9s7A_reW2Si6zlpekLWSayvHP0-Buh62&index=24&t=5389s",
            "https://static3.depositphotos.com/1003596/247/i/950/depositphotos_2477946-stock-photo-erotic-sexy-girl-icon-eblem.jpg",
            "Avenue",
            false,
            false,
            1,
            true,
            true,
            "0886911492")]
        public void PostCreateGameLastStepWhenEverythingIsOkShouldReturnView(
            int time,
            int numberOfPlayers,
            string description,
            int fieldId,
            bool ball,
            string facebookUrl,
            string fieldImageUrl,
            string fieldName,
            bool isPublic,
            bool goalkeeper,
            int adminId,
            bool jerseys,
            bool hasPlaces,
            string phoneNumber)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser((TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<User>(user => user
                            .Add(new User
                            {
                                PhoneNumber = phoneNumber,
                                Id = TestUser.Identifier,
                            }))
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.CreateGameLastStep(new CreateGameFormModel
                {
                    Time = time,
                    NumberOfPlayers = numberOfPlayers,
                    Date = DateTime.Now.Date.AddDays(1),
                    Description = description,
                    FieldId = fieldId,
                    Ball = ball,
                    FacebookUrl = facebookUrl,
                    FieldPhotoPath = fieldImageUrl,
                    FieldName = fieldName,
                    Goalkeeper = goalkeeper,
                    Jerseys = jerseys,

                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Game>(game => game
                        .Any(g =>
                            g.Description == description &&
                            g.FacebookUrl == facebookUrl &&
                            g.Ball == ball &&
                            g.Jerseys == jerseys &&
                            g.Date == DateTime.Now.Date.AddDays(1) &&
                            g.NumberOfPlayers == numberOfPlayers &&
                            g.Time == time &&
                            g.FieldId == fieldId &&
                            g.AdminId == adminId &&
                            g.HasPlaces == hasPlaces &&
                            g.Places == numberOfPlayers &&
                            g.IsPublic == isPublic &&
                            g.PhoneNumber == phoneNumber)))
                
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Mine);

        [Theory]
        [InlineData("1", "Avenue")]
        public void GetEditWhenUserIsNotAdminShouldRedirectToBecomeAdmin(string id, string name)
            => MyController<GamesController>
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
        [InlineData("asdasdasdasdasdasdasd", "Kortove")]
        public void GetEditWhenInfoDoesNotExistShouldRedirectToHomeError(string id, string name)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser((TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Game>(game => game
                            .Add(Games.NewGame()))))
                .Calling(c => c.Edit(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Home.Error, Home.ControllerName);

        [Theory]
        [InlineData("asdasdasdasdasdasdasd", "Kortove")]
        public void GetEditWhenGameDoesNotExistShouldRedirectToHomeError(string id, string name)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser((TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))))
                .Calling(c => c.Edit(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(Home.Error, Home.ControllerName);

        [Theory]
        [InlineData("asdasdasdasdasdasdasd", "Avenue")]
        public void GetEditWhenEverythingIsOkShouldReturnView(string id, string name)
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser((TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<Field>(admin => admin
                            .Add(Fields.Avenue()))
                        .WithSet<Country>(countries => countries
                            .Add(Countries.NewCountry()))
                        .WithSet<City>(city => city
                            .Add(Cities.Haskovo()))
                        .WithSet<Game>(game => game
                            .Add(Games.NewGame()))))
                .Calling(c => c.Edit(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostEditWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Edit(new GameEditServiceModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Theory]
        [InlineData("asdasdasdasdasdasdasd", "Avenue")]
        public void GetDeleteWhenUserIsNotAdminShouldRedirectToBecomeAdmin(string id, string name)
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Delete(id, name))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void PostDeleteWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Delete(new GameDeleteServiceModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void GetMineWhenUserIsNotAdminShouldRedirectToBecomeAdmin()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

    }
}
