namespace MiniFootball.Areas.Admin.Controllers
{
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

        public AdminsController(IAdminService admins)
        {
            this.admins = admins;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (admins.IsAdmin(User.Id()) || User.IsManager())
            {
                TempData[GlobalMessageKey] = Admin.CanNotBecomeAdmin;
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

            TempData[GlobalMessageKey] = Admin.SuccessfullyBecome;
            return Redirect(Home.HomePage);
        }
    }
}