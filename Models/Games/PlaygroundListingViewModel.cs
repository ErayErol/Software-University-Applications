namespace MessiFinder.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Playground;

    public class PlaygroundListingViewModel
    {
        public string Name { get; set; }

        public string Country { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength, ErrorMessage = Range)]
        public string Town { get; set; }

        [Display(Name = "Playground")]
        public int PlaygroundId { get; set; }

        public IEnumerable<PlaygroundListingViewModel> Playgrounds { get; set; }
    }
}
