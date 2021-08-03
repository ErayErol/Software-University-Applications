namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Game;

    public class GameFormModel
    {
        [Display(Name = "Field")]
        public int FieldId { get; set; }

        [Required(ErrorMessage = Empty)]
        public System.DateTime? Date { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        [Display(Name = "Number of players")]
        public int? NumberOfPlayers { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }
    }
}
