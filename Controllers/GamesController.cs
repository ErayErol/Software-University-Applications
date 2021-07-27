namespace MessiFinder.Controllers
{
    using Data;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Services.Countries;
    using Services.Games;
    using System.Linq;

    public class GamesController : Controller
    {
        private readonly MessiFinderDbContext data;
        private readonly ICountryService country;
        private readonly IGameService games;

        public GamesController(MessiFinderDbContext data, ICountryService country, IGameService games)
        {
            this.data = data;
            this.country = country;
            this.games = games;
        }

        [Authorize]
        public IActionResult CountryListing()
        {
            if (this.UserIsAdmin() == false)
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
            gameForm.Town =
                gameForm.Town[0].ToString().ToUpper()
                + gameForm.Town.Substring(1, gameForm.Town.Length - 1).ToLower();

            return View(new PlaygroundListingViewModel
            {
                Playgrounds = this.games.PlaygroundsListing(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaygroundListing(PlaygroundListingViewModel gamePlaygroundModel)
        {
            if (this.games.PlaygroundExist(gamePlaygroundModel.PlaygroundId) == false)
            {
                this.ModelState.AddModelError(nameof(gamePlaygroundModel.PlaygroundId), "Playground does not exist!");
            }

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        public IActionResult Create(PlaygroundListingViewModel gameForm)
        {
            if (this.UserIsAdmin() == false)
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
            var adminId = this.data
                .Admins
                .Where(d => d.UserId == this.User.Id())
                .Select(d => d.Id)
                .FirstOrDefault();

            if (adminId == 0)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

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

            var towns = this.games.Towns();

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

        private bool UserIsAdmin()
            => this.data
                .Admins
                .Any(d => d.UserId == this.User.Id());
    }
}
