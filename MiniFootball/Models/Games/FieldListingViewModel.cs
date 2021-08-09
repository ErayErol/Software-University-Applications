namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Games.Models;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class FieldListingViewModel
    {
        public string Name { get; set; }

        public string CountryName { get; set; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        public string CityName { get; set; }

        [Display(Name = "Select field:")]
        public int FieldId { get; set; }

        public IEnumerable<FieldListingServiceModel> Fields { get; set; }
    }
}
