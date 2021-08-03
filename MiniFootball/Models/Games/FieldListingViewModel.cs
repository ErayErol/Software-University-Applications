namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Services.Games.Models;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class FieldListingViewModel
    {
        public string Name { get; set; }

        public string Country { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength, ErrorMessage = Range)]
        public string Town { get; set; }

        [Display(Name = "Select playground:")]
        public int FieldId { get; set; }

        public IEnumerable<FieldListingServiceModel> Fields { get; set; }
    }
}
