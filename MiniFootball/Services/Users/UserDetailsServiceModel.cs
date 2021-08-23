namespace MiniFootball.Services.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data;
    using static Data.DataConstants;

    public class UserDetailsServiceModel
    {
        public string Id { get; set; }

        [Display(Name = User.Image)]
        public string ImageUrl { get; set; }

        [Display(Name = DataConstants.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Display(Name = User.FirstName)]
        public string FirstName { get; set; }

        [Display(Name = User.LastName)]
        public string LastName { get; set; }

        [Display(Name = User.NickName)]
        public string NickName { get; set; }

        public DateTime Birthdate { get; set; }
        
        public int Age { get; set; }
    }
}
