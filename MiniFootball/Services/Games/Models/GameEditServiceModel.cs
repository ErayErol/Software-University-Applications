namespace MiniFootball.Services.Games.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Game;

    public class GameEditServiceModel
    {
        public string GameId { get; set; }

        [Display(Name = "Select Date:")]
        [Required(ErrorMessage = Empty)]
        public System.DateTime? Date { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(TimeMin, TimeMax)]
        [Display(Name = "Set time:")]
        public int? Time { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        [Display(Name = "Number of players")]
        public int? NumberOfPlayers { get; set; }

        [Display(Name = "Facebook URL")]
        [Required(ErrorMessage = Empty)]
        [Url(ErrorMessage = Url)]
        public string FacebookUrl { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }
    }
}
