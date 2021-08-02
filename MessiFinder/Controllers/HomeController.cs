namespace MessiFinder.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models.Home;
    using Services.Games;
    using Services.Statistics;

    public class HomeController : Controller
    {
        private readonly IGameService game;
        private readonly IStatisticsService statistics;

        public HomeController(
            IGameService game,
            IStatisticsService statistics)
        {
            this.game = game;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            var games = this.game
                .Latest()
                .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                Games = games,
                TotalGames = totalStatistics.TotalGames,
                TotalPlaygrounds = totalStatistics.TotalPlaygrounds,
                TotalUsers = totalStatistics.TotalUsers,
            });
        }

        public IActionResult Error() => View();
    }
}
