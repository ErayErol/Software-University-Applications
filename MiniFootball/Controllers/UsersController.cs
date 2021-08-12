namespace MiniFootball.Controllers
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Admins;
    using Services.Games;
    using Services.Users;

    using static WebConstants;

    public class UsersController : Controller
    {
        private readonly IUserService users;
        private readonly IGameService games;
        private readonly IAdminService admins;
        private readonly MiniFootballDbContext data;

        public UsersController(IUserService users, MiniFootballDbContext data, IAdminService admins, IGameService games)
        {
            this.users = users;
            this.data = data;
            this.admins = admins;
            this.games = games;
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            User user = this.users.User(id);

            return View(user);
        }

        [Authorize]
        public IActionResult ExitGame(string id)
        {
            var userId = this.User.Id();
            var split = id.Split('*');
            var gameId = split[0];
            var userIdToDelete = split[1];
            var adminId = this.admins.IdByUser(userId);

            if (this.admins.IsAdmin(userId) == false 
                && this.User.IsManager() == false 
                && userId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = "You can not remove player from this game!";
                return RedirectToAction(nameof(GamesController.All), "Games");
            }

            if (adminId == 0 
                && this.User.IsManager() == false 
                && userId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = "You can not remove player from this game!";
                return RedirectToAction(nameof(GamesController.All), "Games");
            }

            if (this.games.IsByAdmin(gameId, adminId) == false 
                && this.User.IsManager() == false 
                && userId != userIdToDelete)
            {
                TempData[GlobalMessageKey] = "You can not remove player from this game!";
                return RedirectToAction(nameof(GamesController.All), "Games");
            }

            var userGame = this.data
                .UserGames
                .FirstOrDefault(ug => ug.GameId == gameId && ug.UserId == userIdToDelete);

            if (userGame == null)
            {
                return BadRequest();
            }

            this.data.UserGames.Remove(userGame);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "User exit from game!";
            return RedirectToAction("All", "Games");
        }
    }
}
