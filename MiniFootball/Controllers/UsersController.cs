namespace MiniFootball.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Users;

    public class UsersController : Controller
    {
        private readonly IUserService users;
        
        public UsersController(IUserService users)
        {
            this.users = users;
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var user = users.UserDetails(id);

            return View(user);
        }
    }
}
