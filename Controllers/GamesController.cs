namespace MessiFinder.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Data.Models;

    public class GamesController : Controller
    {
        private readonly MessiFinderDbContext data;

        public GamesController(MessiFinderDbContext data)
            => this.data = data;

        public IActionResult CountryListing()
            => View(new CountryListingFormModel()
            {
                Countries = GetCountries(),
            });

        [HttpPost]
        public IActionResult CountryListing(CountryListingFormModel gameForm)
        {
            if (ModelState.IsValid == false)
            {
                gameForm.Countries = GetCountries();
                return (IActionResult)View(gameForm);
            }

            return RedirectToAction("PlaygroundListing", "Games", gameForm);
        }

        public IActionResult PlaygroundListing(CountryListingFormModel gameForm)
        {
            return View(new PlaygroundListingViewModel()
            {
                Playgrounds = this.GetPlaygroundViewModels(gameForm.Town, gameForm.Country),
                Town = gameForm.Town,
                Country = gameForm.Country,
            });
        }

        [HttpPost]
        public IActionResult PlaygroundListing(PlaygroundListingViewModel gamePlaygroundModel)
        {
            if (this.data.Playgrounds.Any(p => p.Id == gamePlaygroundModel.PlaygroundId) == false)
            {
                this.ModelState.AddModelError(nameof(gamePlaygroundModel.PlaygroundId), "Playground does not exist!");
            }

            return RedirectToAction("GameCreate", "Games", gamePlaygroundModel);
        }

        public IActionResult GameCreate(PlaygroundListingViewModel gameForm)
        {
            return View(new GameCreateFormModel()
            {
                PlaygroundId = gameForm.PlaygroundId,
            });
        }

        [HttpPost]
        public IActionResult GameCreate(GameCreateFormModel gameCreateModel)
        {
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
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();

            return RedirectToAction("GameAll", "Games");

        }

        public IActionResult GameAll()
        {
            var playgrounds = this.data
                .Games
                .Select(p => new GameAllViewModel()
                {
                    Playground = p.Playground,
                    Date = p.Date,
                });

            return View(playgrounds);
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

        public static IEnumerable<string> GetCountries()
        {
            var cultureList = new List<string>();

            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);

                if (cultureList.Contains(getRegionInfo.EnglishName) == false)
                {
                    cultureList.Add(getRegionInfo.EnglishName);
                }
            }

            cultureList.Sort();
            return cultureList;
        }
    }
}
