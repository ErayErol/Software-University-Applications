namespace MessiFinder.Models.Games
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class GameCreateFormModel
    {
        [Display(Name = "Playground")]
        public int PlaygroundId { get; set; }

        [Required(ErrorMessage = ErrorEmptyValue)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = ErrorEmptyValue)]
        [Range(PlaygroundMinNumberOfPlayers, PlaygroundMaxNumberOfPlayers)]
        public int? NumberOfPlayers { get; set; }

        public bool WithGoalkeeper { get; set; }

        [Required(ErrorMessage = ErrorEmptyValue)]
        [StringLength(DefaultMaxDescription, MinimumLength = DefaultMinDescriptionLength, ErrorMessage = ErrorRange)]
        public string Description { get; set; }
    }
}
