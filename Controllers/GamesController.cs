namespace MessiFinder.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;

    public class GamesController : Controller
    {
        private readonly MessiFinderDbContext data;

        public GamesController(MessiFinderDbContext data)
            => this.data = data;

        public IActionResult Create()
            => View(new CreateGameFormModel
            {
                Playgrounds = this.GetPlaygroundViewModels()
            });

        [HttpPost]
        public IActionResult Create(CreateGameFormModel gameForm)
        {
            if (ModelState.IsValid == false)
            {
                gameForm.Playgrounds = this.GetPlaygroundViewModels();

                return View(gameForm);
            }

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<GamePlaygroundViewModel> GetPlaygroundViewModels()
            => this.data
                .Playgrounds
                .Select(x => new GamePlaygroundViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
    }
}
