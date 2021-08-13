namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class CreateGameCountryAndCityViewModel
    {
        [Display(Name = "Select Country:")]
        public string CountryName { get; set; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = "City")]
        public string CityName { get; set; }
    }
}
