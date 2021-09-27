namespace MiniFootball.Areas.Identity.Pages.Account.Manage
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MiniFootball.Data.Models;
    using Services.Users;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using static Convert;
    using static Data.DataConstants.User;

    public partial class IndexModel : PageModel
    {
        private readonly IUserService users;
        private readonly UserManager<User> userManager;
        private readonly INotyfService notifications;

        public IndexModel(UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          IUserService users,
                          INotyfService notifications)
        {
            this.userManager = userManager;
            this.users = users;
            this.notifications = notifications;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Id { get; set; }

            public string Email { get; set; }

            [Display(Name = "First Name")]
            [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
            public string LastName { get; set; }

            [Display(Name = "Nick Name")]
            [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
            public string NickName { get; set; }

            public DateTime Birthdate { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            public IFormFile Photo { get; set; }

            public string PhotoPath { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid)
            {
                if (users.Edit(user, Input))
                {
                    notifications.Success("Your profile has been updated");
                }
            }
            else
            {
                user.FirstName = ToTitleCase(user.FirstName);
                user.LastName = ToTitleCase(user.LastName);
                user.NickName = ToTitleCase(user.NickName);
            }

            await LoadAsync(user);
            return Page();
        }

        private async Task LoadAsync(User user)
        {
            Input ??= new InputModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                PhoneNumber = user.PhoneNumber,
                Birthdate = user.Birthdate,
                PhotoPath = user.PhotoPath,
            };
        }
    }
}
