namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class CreateGameFirstStepViewModel
    {
        [Display(Name = "Select Country:")]
        public string Country { get; set; }

        public IEnumerable<string> Countries { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength, ErrorMessage = Range)]
        public string Town { get; set; }
    }
}
