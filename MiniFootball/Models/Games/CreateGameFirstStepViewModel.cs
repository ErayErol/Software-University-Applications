namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class CreateGameFirstStepViewModel
    {
        [Display(Name = "Select Country:")]
        public string CountryName { get; set; }

        public IEnumerable<string> Countries { get; set; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = "City")]
        public string CityName { get; set; }
    }
}
