namespace MiniFootball.Services.Games.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class CreateGameFormModel : GameEditServiceModel
    {
        [Display(Name = Field.FieldName)]
        public int FieldId { get; set; }

        public int Places
            => GetPlaces;

        public bool HasPlaces
            => true;

        private int GetPlaces
            => NumberOfPlayers.Value;
    }
}