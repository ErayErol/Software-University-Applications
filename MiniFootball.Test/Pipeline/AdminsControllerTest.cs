namespace MiniFootball.Test.Pipeline
{
    using MiniFootball.Controllers;
    using MiniFootball.Data.Models;
    using Models.Admins;
    using Models.Games;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Areas.Admin.Controllers;
    using Xunit;
    using static WebConstants;

    public class AdminsControllerTest
    {
        [Fact]
        public void BecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Admin/Admins/Become")
                    .WithUser())
                .To<AdminsController>(c => c.Become())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Admin")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidMode(string adminName)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Admins/Become")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        Name = adminName,
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<AdminsController>(c => c.Become(new BecomeAdminFormModel()
                {
                    Name = adminName,
                }))
                .Which()
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
                .RedirectToAction(nameof(GamesController.All), "Games");
    }
}
