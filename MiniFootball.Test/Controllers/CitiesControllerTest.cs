namespace MiniFootball.Test.Controllers
{
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Cities;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

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
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(AdminsController.Become), "Admins");

        [Fact]
        public void GetCreateWhenUserIsAdminShouldReturnView()
            => MyController<CitiesController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(new Admin
                            {
                                Name = TestUser.Username,
                                UserId = TestUser.Identifier
                            }))))
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
    }
}
