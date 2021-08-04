namespace MiniFootball.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Models.Home;
    using Services.Games;
    using Services.Games.Models;
    using Services.Statistics;

    public class HomeController : Controller
    {
        private readonly IGameService games;
        private readonly IStatisticsService statistics;
        private readonly IMemoryCache cache;

        public HomeController(
            IGameService games,
            IStatisticsService statistics,
            IMemoryCache cache)
        {
            this.games = games;
            this.statistics = statistics;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            const string latestGamesCacheKey = "LatestGamesCacheKey";

            var lastGames = this.cache.Get<List<GameListingServiceModel>>(latestGamesCacheKey);

            if (lastGames == null)
            {
                lastGames = this.games
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(latestGamesCacheKey, lastGames, cacheOptions);
            }

            var totalStatistics = this.statistics.Total();

            // TODO: Add CSS Number Counter in Statistics and Game Details have to render Details

            return View(new IndexViewModel
            {
                Games = lastGames,
                TotalGames = totalStatistics.TotalGames,
                TotalFields = totalStatistics.TotalFields,
                TotalUsers = totalStatistics.TotalUsers,
            });
        }

        public IActionResult Error() => View();
    }
}
