namespace MessiFinder.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class CountryListingFormModel
    {
        public IEnumerable<string> Countries { get; set; }

        public string Country { get; set; }

        [Required]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        //[RegularExpression("[a-zA-Z]+", ErrorMessage = "")]
        public string Town { get; set; }
    }
}
