namespace MiniFootball.Test.Routing
{
    using MiniFootball.Controllers.Api;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    using static GlobalConstants.Api;

    public class ApiStatisticsControllerTest
    {
        [Fact]
        public void GetStatisticsRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/{ApiStatisticsRoute}"))
                .To<StatisticsApiController>(c => c.GetStatistics());
    }
}
