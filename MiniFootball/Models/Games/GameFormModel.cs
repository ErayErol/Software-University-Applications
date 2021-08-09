namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Game;

    public class GameFormModel
    {
        public string Id { get; set; }

        [Display(Name = "Field")]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Required(ErrorMessage = Empty)]
        public System.DateTime? Date { get; set; }

        [Required(ErrorMessage = Empty)]
        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        [Display(Name = "Number of players")]
        public int? NumberOfPlayers { get; set; }
        
        [Display(Name = "Phone Number")]
        //[Required(ErrorMessage = Empty)]
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Facebook URL")]
        [Required(ErrorMessage = Empty)]
        [Url]
        public string FacebookUrl { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        [Display(Name = "Free places")]
        public int Places { get; set; }

        public bool HasPlaces
            => this.IsHasPlaces;

        public bool IsUserAlreadyJoin { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }

        private bool IsHasPlaces
            => this.Places > 0;
    }
}
