namespace MiniFootball.Areas.Identity.Pages.Account
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MiniFootball.Data.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using static Convert;
    using static Data.DataConstants.User;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly INotyfService notifications;

        public RegisterModel(
            UserManager<User> userManager, 
            INotyfService notifications)
        {
            this.userManager = userManager;
            this.notifications = notifications;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Display(Name = Data.DataConstants.User.FirstName)]
            [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = Data.DataConstants.User.LastName)]
            [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
            public string LastName { get; set; }

            [Display(Name = Data.DataConstants.User.NickName)]
            [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
            public string NickName { get; set; }

            [Required]
            public DateTime? Birthdate { get; set; }

            [Required]
            [Display(Name = Data.DataConstants.ImageUrl)]
            [Url(ErrorMessage = Data.DataConstants.ErrorMessages.Url)]
            public string ImageUrl { get; set; }

            [Required]
            [Display(Name = Data.DataConstants.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = Data.DataConstants.Register.ConfirmPassword)]
            [Compare("Password", ErrorMessage = Data.DataConstants.Register.NotMatchPassword)]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                Input.FirstName = ToTitleCase(Input.FirstName);
                Input.LastName = ToTitleCase(Input.LastName);
                Input.NickName = ToTitleCase(Input.NickName);

                var user = new User
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    NickName = Input.NickName,
                    Birthdate = Input.Birthdate,
                    ImageUrl = Input.ImageUrl,
                    PhoneNumber = Input.PhoneNumber,
                };

                var result = await userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    notifications.Success(Data.DataConstants.Register.SuccessfullyRegister);
                    return Redirect("Login");
                }

                foreach (var error in result.Errors)
                {
                    TempData[WebConstants.GlobalMessageKey] = error.Description;
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}