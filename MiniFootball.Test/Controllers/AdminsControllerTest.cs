namespace MiniFootball.Test.Controllers
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
        [Theory]
        [InlineData("12345")]
        public void GetBecomeShouldBeForAuthorizedUsers(string userId)
            => MyController<AdminsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .WithIdentifier(userId))
                    .WithData(data => data
                        .WithSet<Admin>(admin => admin
                            .Add(new Admin
                            {
                                Name = TestUser.Username,
                                UserId = userId
                            }))))
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .TempData(tempDate => tempDate
                    .ContainingEntryWithKey(GlobalMessageKey))
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
                    .To<GamesController>(c => c.All(With.Any<GameAllQueryModel>())));
    }
}
