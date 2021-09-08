namespace MiniFootball.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MiniFootball.Data.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using static Convert;
    using static Data.DataConstants.User;

    public partial class IndexModel : PageModel
    {
        private readonly MiniFootballDbContext data;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public IndexModel(UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          IWebHostEnvironment hostEnvironment,
                          MiniFootballDbContext data)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.hostEnvironment = hostEnvironment;
            this.data = data;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
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

            if (!ModelState.IsValid)
            {
                user.FirstName = ToTitleCase(user.FirstName);
                user.LastName = ToTitleCase(user.LastName);
                user.NickName = ToTitleCase(user.NickName);

                await LoadAsync(user);
                return Page();
            }

            user.PhoneNumber = Input.PhoneNumber;
            user.Email = Input.Email;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.NickName = Input.NickName;
            user.PhoneNumber = Input.PhoneNumber;
            user.Birthdate = Input.Birthdate.Value;

            if (Input.Photo != null)
            {
                Input.PhotoPath = ProcessUploadFile(Input);
                user.PhotoPath = Input.PhotoPath;
            }
            else
            {
                Input.PhotoPath = user.PhotoPath;
            }

            data.Users.Update(user);
            await data.SaveChangesAsync();

            StatusMessage = "Your profile has been updated";
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
                Birthdate = user.Birthdate.Value,
                PhotoPath = user.PhotoPath,
            };
        }

        private string ProcessUploadFile(InputModel inputModel)
        {
            string uniqueFileName = null;

            if (inputModel.Photo != null)
            {
                var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + inputModel.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                inputModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return uniqueFileName;
        }
    }
}
