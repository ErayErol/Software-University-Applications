namespace MiniFootball.Test.Routing
{
    using Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/"))
                .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Home/Error"))
                .To<HomeController>(c => c.Error());
    }
}
