namespace MiniFootball.Test.Controllers
{
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Admins;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Areas.Admin.Controllers;
    using Data;
    using Models.Games;
    using Xunit;
    using static GlobalConstant.Home;
    using static WebConstants;

    public class AdminsControllerTest
    {
        [Fact]
        public void GetBecomeWhenUserIsAdminShouldReturnView()
            => MyController<AdminsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(TestUser.Identifier))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(Admins.NewAdmin()))))
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .TempData(tempDate => tempDate
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void GetBecomeWhenUserIsNotAdminShouldReturnView()
            => MyController<AdminsController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Admin")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidMode(string adminName)
            => MyController<AdminsController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Become(new BecomeAdminFormModel
                {
                    Name = adminName,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Admin>(admin => admin
                        .Any(a =>
                            a.Name == adminName &&
                            a.UserId == TestUser.Identifier)))
                .TempData(tempDate => tempDate
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(HomePage);
    }
}
