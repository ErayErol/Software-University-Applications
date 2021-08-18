namespace MiniFootball.Test.Routing
{
    using MiniFootball.Controllers;
    using Models.Cities;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CitiesControllerTest
    {
        [Fact]
        public void GetCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Cities/Create"))
                .To<CitiesController>(c => c.Create());

        [Fact]
        public void PostCreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Cities/Create")
                .WithMethod(HttpMethod.Post))
                .To<CitiesController>(c => c.Create(With.Any<CityFormModel>()));
    }
}
