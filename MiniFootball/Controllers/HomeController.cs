namespace MiniFootball.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models.Home;
    using Services.Games;
    using Services.Statistics;

    public class HomeController : Controller
    {
        private readonly IGameService games;
        private readonly IStatisticsService statistics;

        public HomeController(
            IGameService games,
            IStatisticsService statistics)
        {
            this.games = games;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            var lastGames = this.games
                .Latest()
                .ToList();

            var totalStatistics = this.statistics.Total();

            // TODO: Add CSS Number Counter in Statistics and Game Details have to render Details

            return View(new IndexViewModel
            {
                Games = lastGames,
                TotalGames = totalStatistics.TotalGames,
                TotalPlaygrounds = totalStatistics.TotalPlaygrounds,
                TotalUsers = totalStatistics.TotalUsers,
            });
        }

        public IActionResult Error() => View();
    }
}
