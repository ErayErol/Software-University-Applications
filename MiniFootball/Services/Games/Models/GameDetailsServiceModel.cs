namespace MiniFootball.Services.Games.Models
{
    using System.ComponentModel.DataAnnotations;
    using Data;

    public class GameDetailsServiceModel : CreateGameFormModel
    {
        public string UserId { get; set; }

        [Display(Name = DataConstants.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool IsUserAlreadyJoin { get; set; }

        [Display(Name = DataConstants.Game.AvailablePlaces)]
        public virtual int Places 
            => AvailablePlaces;

        public int JoinedPlayersCount { get; set; }

        private int AvailablePlaces
            => NumberOfPlayers.Value - JoinedPlayersCount;
    }
}
