namespace MiniFootball.Services.Games.Models
{
    using System.ComponentModel.DataAnnotations;

    public class GameDetailsServiceModel : CreateGameFormModel
    {
        public string UserId { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public bool IsUserAlreadyJoin { get; set; }

        // TODO: Maybe this 3 props can go to GameEditServiceModel

        [Display(Name = "Available Places")]
        public virtual int Places 
            => AvailablePlaces;

        public int JoinedPlayersCount { get; set; }

        private int AvailablePlaces
            => NumberOfPlayers.Value - JoinedPlayersCount;
    }
}
