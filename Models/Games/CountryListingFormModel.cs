namespace MessiFinder.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class CountryListingFormModel
    {
        public string Country { get; set; }

        public IEnumerable<string> Countries { get; set; }

        [Required]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        public string Town { get; set; }
    }
}
