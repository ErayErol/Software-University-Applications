namespace MessiFinder.Controllers
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Admins;

    public class AdminsController : Controller
    {
        private readonly MessiFinderDbContext data;

        public AdminsController(MessiFinderDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Become()
        {
            if (this.UserIsAdmin())
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

            var adminData = new Admin
            {
                Name = admin.Name,
                PhoneNumber = admin.PhoneNumber,
                UserId = userId
            };

            this.data.Admins.Add(adminData);
            this.data.SaveChanges();

            return RedirectToAction("All", "Games");
        }

        private bool UserIsAdmin()
            => this.data
                .Admins
                .Any(d => d.UserId == this.User.Id());
    }
}