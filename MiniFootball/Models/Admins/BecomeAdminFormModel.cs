namespace MiniFootball.Models.Admins
{
    using System.ComponentModel.DataAnnotations;
    
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Admin;

    public class BecomeAdminFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = Range)]
        public string Name { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
