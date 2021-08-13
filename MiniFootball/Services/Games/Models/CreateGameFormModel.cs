namespace MiniFootball.Services.Games.Models
{
    using Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class CreateGameFormModel : GameEditServiceModel
    {
        [Display(Name = "Field")]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        public int Places
            => GetPlaces;

        public bool HasPlaces
            => true;

        private int GetPlaces
            => NumberOfPlayers.Value;
    }
}