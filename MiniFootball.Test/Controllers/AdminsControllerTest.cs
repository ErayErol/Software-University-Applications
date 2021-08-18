namespace MiniFootball.Test.Controller
{
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Admins;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Models.Games;
    using Xunit;
    using static WebConstants;


    public class AdminsControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsers()
            => MyController<AdminsController>
                .Instance()
                .Calling(c => c.Become(new BecomeAdminFormModel()))
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
                .Calling(c => c.Become(new BecomeAdminFormModel()
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
                .Redirect(redirect => redirect
                    .To<GamesController>(c=>c.All(With.Any<GameAllQueryModel>())));
    }
}
