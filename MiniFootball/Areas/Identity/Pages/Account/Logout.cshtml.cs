namespace MiniFootball.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using MiniFootball.Data.Models;
    using System.Threading.Tasks;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using static GlobalConstant.Notifications;

    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly INotyfService notifications;

        public LogoutModel(
            SignInManager<User> signInManager, 
            ILogger<LogoutModel> logger, 
            INotyfService notifications)
        {
            _signInManager = signInManager;
            _logger = logger;
            this.notifications = notifications;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                notifications.Warning(Goodbye, DurationInSeconds);
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
