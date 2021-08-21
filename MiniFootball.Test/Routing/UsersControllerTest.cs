namespace MiniFootball.Test.Routing
{
    using MiniFootball.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class UsersControllerTest
    {
        [Theory]
        [InlineData(TestUser.Identifier)]
        public void GetDetailsRouteShouldBeMapped(string id)
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Users/Details/{id}"))
                .To<UsersController>(c => c.Details(id));
    }
}
