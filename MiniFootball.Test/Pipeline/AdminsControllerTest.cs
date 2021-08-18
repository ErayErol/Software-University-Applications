namespace MiniFootball.Test.Pipeline
{
    using MiniFootball.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class AdminsControllerTest
    {
        [Fact]
        public void BecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Admins/Become")
                    .WithUser())
                .To<AdminsController>(c => c.Become())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
