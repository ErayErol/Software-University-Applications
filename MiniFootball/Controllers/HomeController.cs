namespace MiniFootball.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Games;
    using Services.Games.Models;
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

            var lastGames = cache.Get<List<GameListingServiceModel>>(latestGamesCacheKey);

            if (lastGames == null)
            {
                lastGames = games
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                cache.Set(latestGamesCacheKey, lastGames, cacheOptions);
            }

            // TODO: Add CSS Number Counter in Statistics
            // TODO: Add Top Users, Most Create Fields, and etc.. look at съседски услуги

            return View(lastGames);
        }

        public IActionResult Error() => View();
    }
}
