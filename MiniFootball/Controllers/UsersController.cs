namespace MiniFootball.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Users;

    using static GlobalConstant.Error;
    using static GlobalConstant.GameUser;

    public class UsersController : Controller
    {
        private readonly IUserService users;
        private readonly INotyfService notifications;

        public UsersController(
            IUserService users,
            INotyfService notifications)
        {
            this.users = users;
            this.notifications = notifications;
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var user = users.UserDetails(id);

            if (user == null)
            {
                notifications.Error(UserDoesNotExist);
                return RedirectToAction(Name, Name);
            }

            return View(user);
        }
    }
}
