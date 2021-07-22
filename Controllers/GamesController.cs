namespace MessiFinder.Controllers
{
    using Data;
    using Data.Models;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Services.Countries;
    using System.Collections.Generic;
    using System.Linq;
    using Services.Users;

    public class GamesController : Controller
    {
        private readonly ICountryService countryService;
        private readonly MessiFinderDbContext data;
        private readonly IUserService userService;

        public GamesController(MessiFinderDbContext data, ICountryService countryService, IUserService userService)
        {
            this.data = data;
            this.countryService = countryService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult CountryListing()
        {
            return this.userService.UserIsAdmin() == false
                ? View()
                : View(new CreateGameFirstStepServiceModel { Countries = countryService.GetAll() });
        }

        [Authorize]
        [HttpPost]
        public IActionResult CountryListing(CountryListingFormModel gameForm)
        {
            if (ModelState.IsValid == false)
            {
                gameForm.Countries = countryService.GetAll();
                return View(gameForm);
            }

            return RedirectToAction("PlaygroundListing", "Games", gameForm);
        }

        [Authorize]
        public IActionResult PlaygroundListing(CountryListingFormModel gameForm)
        {
            gameForm.Town =
                gameForm.Town[0].ToString().ToUpper()
                + gameForm.Town.Substring(1, gameForm.Town.Length - 1).ToLower();

            return View(new PlaygroundListingViewModel()
            {
                Playgrounds = this.GetPlaygroundViewModels(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult PlaygroundListing(PlaygroundListingViewModel gamePlaygroundModel)
        {
            if (this.data.Playgrounds.Any(p => p.Id == gamePlaygroundModel.PlaygroundId) == false)
            {
                this.ModelState.AddModelError(nameof(gamePlaygroundModel.PlaygroundId), "Playground does not exist!");
            }

            return RedirectToAction("Create", "Games", gamePlaygroundModel);
        }

        public IActionResult Create(PlaygroundListingViewModel gameForm)
        {
            if (this.userService.UserIsAdmin() == false)
            {
                return View();
            }

            return View(new GameCreateFormModel()
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
                .Where(d => d.UserId == this.userService.GetUser().GetId())
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

            var game = new Game()
            {
                PlaygroundId = gameCreateModel.PlaygroundId,
                Description = gameCreateModel.Description,
                Date = gameCreateModel.Date.Value,
                NumberOfPlayers = gameCreateModel.NumberOfPlayers.Value,
                WithGoalkeeper = gameCreateModel.WithGoalkeeper,
                Ball = gameCreateModel.Ball,
                Jerseys = gameCreateModel.Jerseys,
                AdminId = adminId,
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All(GameAllQueryModel query)
        {
            var gamesQuery = this.data.Games.AsQueryable();

            if (string.IsNullOrWhiteSpace(query.Town) == false)
            {
                gamesQuery = gamesQuery.Where(g => g.Playground.Town == query.Town);
            }

            if (string.IsNullOrWhiteSpace(query.SearchTerm) == false)
            {
                gamesQuery = gamesQuery
                    .Where(g => g.Playground
                        .Name
                        .ToLower()
                        .Contains(query.SearchTerm.ToLower()));
            }

            gamesQuery = query.Sorting switch
            {
                GameSorting.Town => gamesQuery.OrderBy(g => g.Playground.Town),
                GameSorting.PlaygroundName => gamesQuery.OrderBy(g => g.Playground.Name),
                GameSorting.DateCreated or _ => gamesQuery.OrderBy(g => g.Id)
            };

            var totalGames = gamesQuery.Count();

            var games = gamesQuery
                .Skip((query.CurrentPage - 1) * GameAllQueryModel.GamesPerPage)
                .Take(GameAllQueryModel.GamesPerPage)
                .Select(p => new GameListingViewModel()
                {
                    Id = p.Id,
                    Playground = p.Playground,
                    Date = p.Date,
                }).AsEnumerable();

            var towns = this.data
                .Playgrounds
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

            query.TotalGames = totalGames;
            query.Games = games;
            query.Towns = towns;

            return View(query);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            return View();
        }

        private IEnumerable<PlaygroundListingViewModel> GetPlaygroundViewModels(string town, string country)
        {
            return this.data
                .Playgrounds
                .Where(x => x.Town == town && x.Country == country)
                .Select(x => new PlaygroundListingViewModel
                {
                    PlaygroundId = x.Id,
                    Name = x.Name,
                }).ToList();
        }
    }
}
