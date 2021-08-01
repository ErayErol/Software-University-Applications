namespace MessiFinder.Controllers
{
    using AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Services.Admins;
    using Services.Countries;
    using Services.Games;
    using Services.Playgrounds;

    public class GamesController : Controller
    {
        private readonly ICountryService countries;
        private readonly IGameService games;
        private readonly IAdminService admins;
        private readonly IPlaygroundService playgrounds;
        private readonly IMapper mapper;

        public GamesController(
            ICountryService countries,
            IGameService games,
            IAdminService admins,
            IPlaygroundService playgrounds,
            IMapper mapper)
        {
            this.countries = countries;
            this.games = games;
            this.admins = admins;
            this.playgrounds = playgrounds;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult CountryListing()
        {
            if (this.admins.IsAdmin(this.User.Id()) == false && this.User.IsManager() == false)
            {
                return View();
            }

            return View(new CreateGameFirstStepViewModel
            {
                Countries = this.countries.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CountryListing(CreateGameFirstStepViewModel gameForm)
        {
            if (ModelState.IsValid == false)
            {
                gameForm.Countries = this.countries.All();
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
                Playgrounds = this.playgrounds.PlaygroundsListing(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaygroundListing(PlaygroundListingViewModel gamePlaygroundModel)
        {
            if (this.playgrounds.PlaygroundExist(gamePlaygroundModel.PlaygroundId) == false)
            {
                this.ModelState.AddModelError(nameof(gamePlaygroundModel.PlaygroundId), "Playground does not exist!");
            }

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        public IActionResult Create(PlaygroundListingViewModel gameForm)
        {
            if (this.admins.IsAdmin(this.User.Id()) == false && this.User.IsManager() == false)
            {
                return View();
            }

            return View(new GameFormModel
            {
                PlaygroundId = gameForm.PlaygroundId,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(GameFormModel gameCreateModel)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

            // TODO: There is already exist game in this playgrounds in this date

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

            var towns = this.playgrounds.Towns();

            query.TotalGames = queryResult.TotalGames;
            query.Games = queryResult.Games;
            query.Towns = towns;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myGames = this.games.ByUser(this.User.Id());

            return View(myGames);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            // TODO: This edit only Game, but playground is same 
            // When we are in edit page, add button for edit playground
            // and then return RedirectToAction(nameof(EditPlayground));
            var userId = this.User.Id();

            if (this.admins.IsAdmin(userId) == false && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            var game = this.games.Details(id);

            if (game.UserId != userId && this.User.IsManager() == false)
            {
                return Unauthorized();
            }

            var gameForm = this.mapper.Map<GameFormModel>(game);

            return View(gameForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, GameFormModel game)
        {
            var adminId = this.admins.IdByUser(this.User.Id());

            if (adminId == 0 && this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            // TODO: Do this check for EditPlayground
            //if (this.playgrounds.PlaygroundExist(game.PlaygroundId) == false)
            //{
            //    this.ModelState.AddModelError(nameof(game.PlaygroundId), "Playground does not exist.");
            //}

            if (!ModelState.IsValid)
            {
                return View(game);
            }

            if (this.games.IsByAdmin(id, adminId) == false && this.User.IsManager() == false)
            {
                return BadRequest();
            }

            this.games.Edit(
                id,
                game.Date,
                game.NumberOfPlayers,
                game.Ball,
                game.Jerseys,
                game.Goalkeeper,
                game.Description);

            return RedirectToAction(nameof(All));
        }
    }
}
