namespace MiniFootball.Areas.Admin.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;
    using Services.Admins;
    using static GlobalConstant;
    using static WebConstants;

    [Area(Admin.AreaName)]
    public class AdminsController : Controller
    {
        private readonly IAdminService admins;
        private readonly INotyfService notifications;

        public AdminsController(
            IAdminService admins,
            INotyfService notifications)
        {
            this.admins = admins;
            this.notifications = notifications;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (admins.IsAdmin(User.Id()) || User.IsManager())
            {
                notifications.Error(Admin.CanNotBecomeAdmin);
                return View();
            }

            return View(new BecomeAdminFormModel
            {
                Name = User.Identity?.Name
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeAdminFormModel adminModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(adminModel);
            }

            admins.Become(adminModel.Name, User.Id());

            notifications.Success(Admin.SuccessfullyBecome);
            return Redirect(Home.HomePage);
        }
    }
}