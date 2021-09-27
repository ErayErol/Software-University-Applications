namespace MiniFootball.Models.Cities
{
    using Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;
    using static Data.DataConstants.Country;

    public class CityFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = Range)]
        [Display(Name = SelectCountry)]
        public string CountryName { get; set; }
        public Country Country { get; set; }

        public IEnumerable<string> Countries { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = Range)]
        public string Name { get; set; }
    }
}
