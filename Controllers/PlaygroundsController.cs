namespace MessiFinder.Controllers
{
    using Data;
    using Data.Models;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Playgrounds;
    using System.Linq;
    using Utilities;

    public class PlaygroundsController : Controller
    {
        private readonly MessiFinderDbContext data;

        public PlaygroundsController(MessiFinderDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Create()
        {
            if (this.UserIsAdmin() == false)
            {
                return View();
            }

            return View(new PlaygroundCreateFormModel()
            {
                Countries = Countries.GetAll(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(PlaygroundCreateFormModel playgroundModel)
        {
            var adminId = this.data
                .Admins
                .Where(d => d.UserId == this.User.GetId())
                .Select(d => d.Id)
                .FirstOrDefault();

            if (adminId == 0)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                playgroundModel.Countries = Countries.GetAll();
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

            var playground = new Playground()
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

        public IActionResult All()
        {
            var playgrounds = this.data
                .Playgrounds
                .Select(p => new PlaygroundAllViewModel()
                {
                    Name = p.Name,
                    Country = p.Country,
                    Town = p.Town,
                    Address = p.Address,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                });

            return View(playgrounds);
        }

        private bool UserIsAdmin()
            => this.data
                .Admins
                .Any(d => d.UserId == this.User.GetId());
    }
}
