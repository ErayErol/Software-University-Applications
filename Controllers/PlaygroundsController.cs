namespace MessiFinder.Controllers
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Models.Playgrounds;
    using System.Linq;

    public class PlaygroundsController : Controller
    {
        private readonly MessiFinderDbContext data;

        public PlaygroundsController(MessiFinderDbContext data)
            => this.data = data;

        public IActionResult PlaygroundCreate()
            => View();

        [HttpPost]
        public IActionResult PlaygroundCreate(PlaygroundCreateFormModel playgroundModel)
        {
            if (ModelState.IsValid == false)
            {
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
                Description = playgroundModel.Description,
            };

            this.data.Playgrounds.Add(playground);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult PlaygroundAll()
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
    }
}
