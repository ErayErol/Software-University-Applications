namespace MessiFinder.Controllers
{
    using Data;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;
    using Services.Admins;

    public class AdminsController : Controller
    {
        private readonly MessiFinderDbContext data;
        private readonly IAdminService admin;

        public AdminsController(MessiFinderDbContext data, IAdminService admin)
        {
            this.data = data;
            this.admin = admin;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (this.admin.IsAdmin(this.User.Id()))
            {
                return View();
            }

            return View(new BecomeAdminFormModel()
            {
                Name = this.User.Identity?.Name
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeAdminFormModel admin)
        {
            if (ModelState.IsValid == false)
            {
                return View(admin);
            }

            var userId = this.User.Id();

            this.admin.Become(admin.Name, admin.PhoneNumber, userId);

            return RedirectToAction("All", "Games");
        }
    }
}