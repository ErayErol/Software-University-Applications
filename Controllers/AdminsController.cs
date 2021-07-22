namespace MessiFinder.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;
    using Services.Admins;
    using Services.Users;

    public class AdminsController : Controller
    {
        private readonly IUserService userService;
        private readonly IAdminService adminService;

        public AdminsController(IUserService userService, IAdminService adminService)
        {
            this.userService = userService;
            this.adminService = adminService;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (this.userService.UserIsAdmin() == false)
            {
                return View(new BecomeAdminFormModel()
                {
                    Name = this.User.Identity?.Name
                });
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeAdminFormModel admin)
        {
            if (ModelState.IsValid == false)
            {
                return View(admin);
            }

            this.adminService.Become(admin);

            return RedirectToAction("All", "Games");
        }
    }
}