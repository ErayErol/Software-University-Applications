namespace MiniFootball.Areas.Manager.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Services.Admins;
    using Services.Fields;

    using static GlobalConstant;

    [Area(Manager.AreaName)]
    public class AdminsController : ManagerController
    {
        private readonly IAdminService admins;

        public AdminsController(IAdminService admins)
        {
            this.admins = admins;
        }

        public IActionResult AllAdmins()
        {
            var admins = this.admins.All();

            return View(admins);
        }

        public IActionResult ChangeVisibility(int id)
        {
            admins.ChangeVisibility(id);

            return RedirectToAction(nameof(AllAdmins));
        }
    }
}