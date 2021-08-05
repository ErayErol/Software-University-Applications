namespace MiniFootball.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;
    using Services.Admins;

    using static WebConstants;

    public class AdminsController : Controller
    {
        private readonly IAdminService admins;

        public AdminsController(IAdminService admins)
        {
            this.admins= admins;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (this.admins.IsAdmin(this.User.Id()) || this.User.IsManager())
            {
                return View();
            }

            return View(new BecomeAdminFormModel
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

            this.admins.Become(admin.Name, admin.PhoneNumber, this.User.Id());

            TempData[GlobalMessageKey] = "You have become an admin!";

            return RedirectToAction("All", "Games");
        }
    }
}