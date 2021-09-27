namespace MiniFootball.Services.Games.Models
{
    public class GameSeePlayersServiceModel : GameUserInfoServiceModel
    {
        public string GameId { get; set; }

        public bool IsCreator { get; set; }
    }
}
