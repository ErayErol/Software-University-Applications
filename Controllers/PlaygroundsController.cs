namespace MessiFinder.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Games;
    using Models.Playgrounds;
    using Services.Admins;
    using Services.Countries;
    using Services.Playgrounds;
    using Services.Users;

    public class PlaygroundsController : Controller
    {
        private readonly IUserService userService;
        private readonly IAdminService adminService;
        private readonly ICountryService countryService;
        private readonly IPlaygroundService playgroundService;

        public PlaygroundsController(
            ICountryService countryService, 
            IUserService userService, 
            IAdminService adminService, 
            IPlaygroundService playgroundService)
        {
            this.countryService = countryService;
            this.userService = userService;
            this.adminService = adminService;
            this.playgroundService = playgroundService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.userService.UserIsAdmin() == false
                ? View()
                : View(new PlaygroundCreateFormModel { Countries = countryService.All(), });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(PlaygroundCreateFormModel playgroundModel)
        {
            var adminId = this.adminService.GetId();

            if (adminId == 0)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                playgroundModel.Countries = this.countryService.All();
                return View(playgroundModel);
            }

            if (this.playgroundService.CheckForSamePlayground(playgroundModel))
            {
                return View(playgroundModel);
            }

            this.playgroundService.Create(playgroundModel, adminId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All(PlaygroundAllQueryModel query)
            => View(this.playgroundService.All(query));
    }
}
