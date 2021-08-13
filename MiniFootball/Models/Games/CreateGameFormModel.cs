namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;
    using Data;
    using Data.Models;

    public class CreateGameFormModel
    {
        public string Id { get; set; }

        [Display(Name = "Field")]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Display(Name = "Select Date:")]
        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        public System.DateTime? Date { get; set; }

        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        [Range(DataConstants.Game.TimeMin, DataConstants.Game.TimeMax)]
        [Display(Name = "Set time:")]
        public int? Time { get; set; }

        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        [Range(DataConstants.Game.NumberOfPlayersMin, DataConstants.Game.NumberOfPlayersMax)]
        [Display(Name = "Number of players")]
        public int? NumberOfPlayers { get; set; }

        [Display(Name = "Facebook URL")]
        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        [Url(ErrorMessage = DataConstants.ErrorMessages.Url)]
        public string FacebookUrl { get; set; }

        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        [StringLength(DataConstants.Game.DescriptionMaxLength, MinimumLength = DataConstants.Game.DescriptionMinLength, ErrorMessage = DataConstants.ErrorMessages.Range)]
        public string Description { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        public int Places
            => this.GetPlaces;

        public bool HasPlaces
            => true;

        private int GetPlaces
            => this.NumberOfPlayers.Value;
    }
}