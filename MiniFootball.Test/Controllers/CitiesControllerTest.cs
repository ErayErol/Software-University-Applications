namespace MiniFootball.Test.Controllers
{
    using Data;
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Cities;
    using Models.Games;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Areas.Admin.Controllers;
    using Xunit;
    using static WebConstants;

    public class CitiesControllerTest
    {
        [Fact]
        public void GetCreateWhenUserIsNotAdminShouldRedirectToActionBecomeAdmin()
            => MyController<CitiesController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .TempData(tempDate => tempDate
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(AdminsController.Become), "Admins");

        [Fact]
        public void GetCreateWhenUserIsAdminShouldReturnView()
            => MyController<CitiesController>
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
        public void PostCreateWhenUserIsNotAdminShouldRedirectToActionBecomeAdmin()
            => MyController<CitiesController>
                .Instance()
                .Calling(c => c.Create(new CityFormModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AdminsController>(c => c.Become()));

        [Fact]
        public void PostCreateWhenUserIsAdminAndModelStateIsInvalidShouldReturnView()
            => MyController<CitiesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier)
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.Create(new CityFormModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Sofia", "Bulgaria", 1, 24)]
        public void PostCreateShouldBeForAuthorizedUsersAndReturnRedirectWithValidMode(
            string name,
            string countryName,
            int id,
            int countryId)
            => MyController<CitiesController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))
                        .WithSet<City>(city => city
                            .Add(Cities.Sofia()))))
                .Calling(c => c.Create(new CityFormModel
                {
                    Name = name,
                    CountryName = countryName,
                    Id = id,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<City>(city => city
                        .Any(c =>
                            c.Name == name &&
                            c.CountryId == countryId &&
                            c.Id == id)))
                .TempData(tempDate => tempDate
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>(c => c.CreateGameFirstStep(With.Any<CreateGameFirstStepViewModel>())));
    }
}
