namespace MiniFootball.Test.Routing
{
    using MiniFootball.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void GetIndexRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/"))
                .To<HomeController>(c => c.Index());
    }
}
