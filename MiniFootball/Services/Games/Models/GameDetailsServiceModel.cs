namespace MiniFootball.Services.Games.Models
{
    public class GameDetailsServiceModel : CreateGameFormModel
    {
        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public string UserId { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsUserAlreadyJoin { get; set; }

        // TODO: Maybe this 3 props can go to GameEditServiceModel

        public virtual int Places 
            => this.AvailablePlaces;

        public int JoinedPlayersCount { get; set; }

        private int AvailablePlaces
            => NumberOfPlayers.Value - JoinedPlayersCount;
    }
}
