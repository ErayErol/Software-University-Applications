namespace MiniFootball.Models.Games
{
    using Services.Games.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class FieldListingViewModel
    {
        public string Name { get; set; }

        public string CountryName { get; set; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = "City")]
        public string CityName { get; set; }

        [Display(Name = "Select field:")]
        public int FieldId { get; set; }

        public IEnumerable<FieldListingServiceModel> Fields { get; set; }
    }
}
