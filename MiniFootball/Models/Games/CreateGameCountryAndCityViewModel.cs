namespace MiniFootball.Models.Games
{
    using Data;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.Country;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class CreateGameCountryAndCityViewModel
    {
        [Display(Name = SelectCountry)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = DataConstants.City.CityName)]
        public string CityName { get; set; }
    }
}
