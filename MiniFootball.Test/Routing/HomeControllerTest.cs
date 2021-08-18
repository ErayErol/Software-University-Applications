namespace MiniFootball.Test.Routing
{
    using Controllers;
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

        [Fact]
        public void GetErrorRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Home/Error"))
                .To<HomeController>(c => c.Error());
    }
}
