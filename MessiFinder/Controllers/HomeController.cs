namespace MessiFinder.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Home;
    using Services.Statistics;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public class HomeController : Controller
    {
        private readonly MessiFinderDbContext data;
        private readonly IStatisticsService statistics;
        private readonly IMapper mapper;

        public HomeController(
            MessiFinderDbContext data, 
            IStatisticsService statistics,
            IMapper mapper)
        {
            this.data = data;
            this.statistics = statistics;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var games = this.data
                .Games
                .ProjectTo<GameIndexViewModel>(this.mapper.ConfigurationProvider)
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
