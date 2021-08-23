namespace MiniFootball.Services.Games.Models
{
    using System.ComponentModel.DataAnnotations;
    using Data;
    using static Data.DataConstants;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Game;

    public class GameEditServiceModel
    {
        public string GameId { get; set; }

        [Display(Name = SelectDate)]
        [Required(ErrorMessage = Empty)]
        public System.DateTime? Date { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(TimeMin, TimeMax)]
        [Display(Name = SelectTime)]
        public int? Time { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        [Display(Name = Game.NumberOfPlayers)]
        public int? NumberOfPlayers { get; set; }

        [Display(Name = PhoneNumber)]
        [Required(ErrorMessage = Empty)]
        [Url(ErrorMessage = Url)]
        public string FacebookUrl { get; set; }

        [Display(Name = Field.FieldImage)]
        public string FieldImageUrl { get; set; }

        [Display(Name = Field.FieldName)]
        public string FieldName { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }
    }
}
