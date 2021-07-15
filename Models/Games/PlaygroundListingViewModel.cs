namespace MessiFinder.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using static Data.DataConstants;

    public class PlaygroundListingViewModel
    {
        public string Name { get; set; }

        public string Country { get; set; }

        [Required]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        public string Town { get; set; }

        [Display(Name = "Playground")]
        public int PlaygroundId { get; set; }

        public IEnumerable<PlaygroundListingViewModel> Playgrounds { get; set; }
    }
}
