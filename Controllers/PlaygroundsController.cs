namespace MessiFinder.Controllers
{
    using Data;
    using Data.Models;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Playgrounds;
    using System.Linq;
    using Models;
    using Services.Countries;

    public class PlaygroundsController : Controller
    {
        private readonly MessiFinderDbContext data;
        private readonly ICountryService country;

        public PlaygroundsController(MessiFinderDbContext data, ICountryService country)
        {
            this.data = data;
            this.country = country;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.UserIsAdmin() == false)
            {
                return View();
            }

            return View(new PlaygroundCreateFormModel
            {
                Countries = this.country.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(PlaygroundCreateFormModel playgroundModel)
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
                playgroundModel.Countries = this.country.All();
                return View(playgroundModel);
            }

            if (this.data.Playgrounds.Any(p =>
                p.Name == playgroundModel.Name &&
                p.Country == playgroundModel.Country &&
                p.Town == playgroundModel.Town &&
                p.Address == playgroundModel.Address))
            {
                // there are already exist playground with this name, country, town, address
                return View(playgroundModel);
            }

            var playground = new Playground
            {
                Name = playgroundModel.Name,
                Country = playgroundModel.Country,
                Town = playgroundModel.Town,
                Address = playgroundModel.Address,
                ImageUrl = playgroundModel.ImageUrl,
                PhoneNumber = playgroundModel.PhoneNumber,
                Parking = playgroundModel.Parking,
                Cafe = playgroundModel.Cafe,
                Shower = playgroundModel.Shower,
                ChangingRoom = playgroundModel.ChangingRoom,
                Description = playgroundModel.Description,
                AdminId = adminId
            };

            this.data.Playgrounds.Add(playground);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] PlaygroundAllQueryModel query)
        {
            var playgroundsQuery = this.data.Playgrounds.AsQueryable();

            if (string.IsNullOrWhiteSpace(query.Town) == false)
            {
                playgroundsQuery = playgroundsQuery.Where(g => g.Town == query.Town);
            }

            if (string.IsNullOrWhiteSpace(query.SearchTerm) == false)
            {
                playgroundsQuery = playgroundsQuery
                    .Where(g => g
                        .Name
                        .ToLower()
                        .Contains(query.SearchTerm.ToLower()));
            }

            playgroundsQuery = query.Sorting switch
            {
                GameSorting.Town => playgroundsQuery.OrderBy(g => g.Town),
                GameSorting.PlaygroundName => playgroundsQuery.OrderBy(g => g.Name),
                GameSorting.DateCreated or _ => playgroundsQuery.OrderBy(g => g.Id)
            };

            var totalPlaygrounds = playgroundsQuery.Count();

            var playgrounds = playgroundsQuery
                .Skip((query.CurrentPage - 1) * PlaygroundAllQueryModel.GamesPerPage)
                .Take(PlaygroundAllQueryModel.GamesPerPage)
                .Select(p => new PlaygroundAllViewModel
                {
                    Town = p.Town,
                    Country = p.Country,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Address = p.Address,
                }).AsEnumerable();

            var towns = this.data
                .Playgrounds
                .Select(p => p.Town)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

            query.TotalPlaygrounds = totalPlaygrounds;
            query.Playgrounds = playgrounds;
            query.Towns = towns;

            return View(query);
        }

        private bool UserIsAdmin()
            => this.data
                .Admins
                .Any(d => d.UserId == this.User.Id());
    }
}
