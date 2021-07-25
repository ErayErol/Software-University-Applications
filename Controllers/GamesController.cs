namespace MessiFinder.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Services.Admins;
    using Services.Countries;
    using Services.Games;
    using Services.Playgrounds;
    using Services.Users;


    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IUserService userService;
        private readonly IAdminService adminService;
        private readonly ICountryService countryService;
        private readonly IPlaygroundService playgroundService;

        public GamesController(
            ICountryService countryService,
            IUserService userService,
            IPlaygroundService playgroundService,
            IAdminService adminService,
            IGameService gameService)
        {
            this.countryService = countryService;
            this.userService = userService;
            this.playgroundService = playgroundService;
            this.adminService = adminService;
            this.gameService = gameService;
        }

        [Authorize]
        public IActionResult CountryListing()
        {
            return this.userService.UserIsAdmin() == false
                ? View()
                : View(new CreateGameFirstStepViewModel { Countries = countryService.All() });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CountryListing(CreateGameFirstStepViewModel gameForm)
        {
            if (ModelState.IsValid == false)
            {
                gameForm.Countries = countryService.All();
                return View(gameForm);
            }

            return RedirectToAction("PlaygroundListing", "Games", gameForm);

        }

        [Authorize]
        public IActionResult PlaygroundListing(CreateGameFirstStepViewModel gameForm)
        {
            gameForm.Town =
                gameForm.Town[0].ToString().ToUpper()
                + gameForm.Town.Substring(startIndex: 1, length: gameForm.Town.Length - 1).ToLower();

            return View(new PlaygroundListingViewModel()
            {
                Playgrounds = this.playgroundService.PlaygroundViewModels(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaygroundListing(PlaygroundListingViewModel gamePlaygroundModel)
        {
            if (this.playgroundService.IsExist(gamePlaygroundModel) == false)
            {
                this.ModelState
                    .AddModelError(nameof(gamePlaygroundModel.PlaygroundId), "Playground does not exist!");
            }

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        public IActionResult Create(PlaygroundListingViewModel gameForm)
            => this.userService.UserIsAdmin() == false
                ? View()
                : View(new GameCreateFormModel { PlaygroundId = gameForm.PlaygroundId, });

        [Authorize]
        [HttpPost]
        public IActionResult Create(GameCreateFormModel gameCreateModel)
        {
            var adminId = this.adminService.GetId();

            if (adminId == 0)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                return View(gameCreateModel);
            }

            this.gameService.Create(gameCreateModel, adminId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All(GameAllQueryModel query)
            => View(this.gameService.All(query));

        [Authorize]
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
