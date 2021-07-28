namespace MessiFinder.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Services.Countries;
    using Services.Games;
    using System.Linq;
    using Services.Admins;
    using Services.Playgrounds;

    public class GamesController : Controller
    {
        private readonly ICountryService country;
        private readonly IGameService games;
        private readonly IAdminService admin;
        private readonly IPlaygroundService playground;

        public GamesController(
            ICountryService country, 
            IGameService games, 
            IAdminService admin, IPlaygroundService playground)
        {
            this.country = country;
            this.games = games;
            this.admin = admin;
            this.playground = playground;
        }

        [Authorize]
        public IActionResult CountryListing()
        {
            if (this.admin.IsAdmin(this.User.Id()) == false)
            {
                return View();
            }

            return View(new CreateGameFirstStepViewModel
            {
                Countries = this.country.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CountryListing(CreateGameFirstStepViewModel gameForm)
        {
            if (ModelState.IsValid == false)
            {
                gameForm.Countries = this.country.All();
                return View(gameForm);
            }

            return RedirectToAction("PlaygroundListing", "Games", gameForm);
        }

        [Authorize]
        public IActionResult PlaygroundListing(CreateGameFirstStepViewModel gameForm)
        {
            // TODO : Do this for all
            gameForm.Town =
                gameForm.Town[0].ToString().ToUpper()
                + gameForm.Town.Substring(1, gameForm.Town.Length - 1).ToLower();

            return View(new PlaygroundListingViewModel
            {
                Playgrounds = this.playground.PlaygroundsListing(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaygroundListing(PlaygroundListingViewModel gamePlaygroundModel)
        {
            if (this.playground.PlaygroundExist(gamePlaygroundModel.PlaygroundId) == false)
            {
                this.ModelState.AddModelError(nameof(gamePlaygroundModel.PlaygroundId), "Playground does not exist!");
            }

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        public IActionResult Create(PlaygroundListingViewModel gameForm)
        {
            if (this.admin.IsAdmin(this.User.Id()) == false)
            {
                return View();
            }

            return View(new GameCreateFormModel
            {
                PlaygroundId = gameForm.PlaygroundId,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(GameCreateFormModel gameCreateModel)
        {
            var adminId = this.admin.IdByUser(this.User.Id());

            if (adminId == 0)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

            // TODO: There is already exist game in this playground in this date

            this.games.Create(
                gameCreateModel.PlaygroundId,
                gameCreateModel.Description,
                gameCreateModel.Date.Value,
                gameCreateModel.NumberOfPlayers.Value,
                gameCreateModel.Goalkeeper,
                gameCreateModel.Ball,
                gameCreateModel.Jerseys,
                adminId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] GameAllQueryModel query)
        {
            var queryResult = this.games.All(
                query.Town,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.GamesPerPage);

            var towns = this.playground.Towns();

            query.TotalGames = queryResult.TotalGames;
            query.Games = queryResult.Games;
            query.Towns = towns;

            return View(query);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
