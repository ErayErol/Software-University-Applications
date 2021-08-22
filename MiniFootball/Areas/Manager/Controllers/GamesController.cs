namespace MiniFootball.Areas.Manager.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Services.Games;

    [Area(ManagerConstants.AreaName)]
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
            this.games.ChangeVisibility(id);

            return RedirectToAction(nameof(AllGames));
        }
    }
}
