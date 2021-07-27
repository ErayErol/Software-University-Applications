namespace MessiFinder.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Home;
    using Services.Statistics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly MessiFinderDbContext data;
        private readonly IStatisticsService statistics;

        public HomeController(
            MessiFinderDbContext data, 
            IStatisticsService statistics)
        {
            this.data = data;
            this.statistics = statistics;
        }

        public IActionResult Index()
        {
            var games = this.data
                .Games
                .Select(p => new GameIndexViewModel
                {
                    Id = p.Id,
                    Playground = p.Playground,
                    Date = p.Date,
                })
                .OrderByDescending(g => g.Id)
                .Take(3)
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
