namespace MiniFootball.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Statistics;

    using static GlobalConstant;

    [ApiController]
    [Route(Api.ApiStatisticsRoute)]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService statistics)
            => this.statistics = statistics;

        [HttpGet]
        public StatisticsServiceModel GetStatistics()
            => statistics.Total();
    }
}
