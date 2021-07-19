namespace MessiFinder.Models.Games
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data;
    using static Data.DataConstants.Game;
    using static Data.DataConstants.ErrorMessages;

    public class GameCreateFormModel
    {
        [Display(Name = "Playground")]
        public int PlaygroundId { get; set; }

        [Required(ErrorMessage = Empty)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        public int? NumberOfPlayers { get; set; }

        public bool WithGoalkeeper { get; set; }
        
        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DataConstants.ErrorMessages.Range)]
        public string Description { get; set; }
    }
}
