namespace MiniFootball.Data.Models
{
    public class UserGame
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}
