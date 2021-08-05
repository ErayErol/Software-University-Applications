namespace MiniFootball.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Games;
    using Services.Games.Models;
    using Services.Statistics;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IGameService games;
        private readonly IMemoryCache cache;

        public HomeController(
            IGameService games,
            IMemoryCache cache)
        {
            this.games = games;
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

            // TODO: Add CSS Number Counter in Statistics

            return View(lastGames);
        }

        public IActionResult Error() => View();
    }
}
