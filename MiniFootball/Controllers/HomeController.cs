namespace MiniFootball.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Games;
    using Services.Games.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static WebConstants.Cache;

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
            var lastGames = cache.Get<List<GameListingServiceModel>>(LatestGamesCacheKey);

            if (lastGames == null)
            {
                lastGames = games
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                cache.Set(LatestGamesCacheKey, lastGames, cacheOptions);
            }

            return View(lastGames);
        }

        public IActionResult Error() => View();
    }
}
