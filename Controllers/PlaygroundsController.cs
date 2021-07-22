namespace MessiFinder.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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

        public PlaygroundsController(MessiFinderDbContext data, ICountryService countryService, IUserService userService, IAdminService adminService, IPlaygroundService playgroundService)
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
                : View(new PlaygroundCreateFormModel { Countries = countryService.GetAll(), });
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
                playgroundModel.Countries = this.countryService.GetAll();
                return View(playgroundModel);
            }

            if (this.playgroundService.CheckForSamePlayground(playgroundModel))
            {
                return View(playgroundModel);
            }

            this.playgroundService.Create(playgroundModel, adminId);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All()
            => View(this.playgroundService.All());
    }
}
