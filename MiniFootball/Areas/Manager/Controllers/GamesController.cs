namespace MiniFootball.Areas.Manager.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Services.Games;
    using static GlobalConstant;

    [Area(Manager.AreaName)]
    public class GamesController : ManagerController
    {
        private readonly IGameService games;

        public GamesController(IGameService games)
        {
            this.games = games;
        }

        public IActionResult AllGames()
        {
            var games = this.games
                .All(publicOnly: false)
                .Games
                .OrderByDescending(g => g.Date);

            return View(games);
        }

        public IActionResult ChangeVisibility(string id)
        {
            games.ChangeVisibility(id);

            return RedirectToAction(nameof(AllGames));
        }
    }
}
