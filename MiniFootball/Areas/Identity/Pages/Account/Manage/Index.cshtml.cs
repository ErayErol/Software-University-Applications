namespace MiniFootball.Areas.Identity.Pages.Account.Manage
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MiniFootball.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using static Data.DataConstants.User;

    using static Convert;
    using static WebConstants;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string LastName { get; set; }

        [Display(Name = "Nick Name")]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string NickName { get; set; }

        [Required]
        public DateTime? Birthdate { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            ImageUrl = user.ImageUrl;
            Email = userName;
            FirstName = user.FirstName;
            LastName= user.LastName;
            NickName = user.NickName;
            PhoneNumber = user.PhoneNumber;
            Birthdate = user.Birthdate.Value;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                user.FirstName = ToTitleCase(user.FirstName);
                user.LastName = ToTitleCase(user.LastName);
                user.NickName = ToTitleCase(user.NickName);

                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
