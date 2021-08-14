namespace MiniFootball.Services.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserDetailsServiceModel
    {
        public string Id { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Nick Name")]
        public string NickName { get; set; }

        public DateTime Birthdate { get; set; }
        
        public int Age { get; set; }
    }
}
