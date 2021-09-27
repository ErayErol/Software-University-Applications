namespace MiniFootball.Areas.Admin.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;
    using Services.Admins;

    using static GlobalConstants;

    [Area(Admin.AreaName)]
    public class AdminsController : Controller
    {
        private readonly IAdminService admins;
        private readonly INotyfService notifications;

        public AdminsController(IAdminService admins,
                                INotyfService notifications)
        {
            this.admins = admins;
            this.notifications = notifications;
        }

        [Authorize]
        public ViewResult Become()
        {
            if (admins.IsAdmin(User.Id()) || User.IsManager())
            {
                notifications.Error(Admin.CanNotBecomeAdmin);
                return View();
            }

            var formModel = new BecomeAdminFormModel
            {
                Name = User.Identity?.Name
            };

            return View(formModel);
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
            notifications.Information("You have successfully stated that you want to become an admin. " +
                                      "Please wait for the approval!");
            return Redirect(Home.HomePage);
        }
    }
}