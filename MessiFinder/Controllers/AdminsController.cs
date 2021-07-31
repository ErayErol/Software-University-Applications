namespace MessiFinder.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;
    using Services.Admins;

    public class AdminsController : Controller
    {
        private readonly IAdminService admin;

        public AdminsController(IAdminService admin)
        {
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