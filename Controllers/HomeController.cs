namespace MessiFinder.Controllers
{
    using Data;
    using MessiFinder.Models;
    using Microsoft.AspNetCore.Mvc;
    using Models.Home;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly MessiFinderDbContext data;

        public HomeController(MessiFinderDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            var games = this.data
                .Games
                .Select(p => new GameIndexViewModel()
                {
                    Id = p.Id,
                    Playground = p.Playground,
                    Date = p.Date,
                })
                .OrderByDescending(g => g.Id)
                .Take(3)
                .ToList();

            var totalGames = this.data.Games.Count();
            var totalPlaygrounds = this.data.Playgrounds.Count();

            return View(new IndexViewModel
            {
                Games = games,
                TotalGames = totalGames,
                TotalPlaygrounds = totalPlaygrounds,
                TotalUsers = 0,
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
