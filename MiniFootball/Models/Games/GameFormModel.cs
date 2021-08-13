namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    public class GameFormModel : CreateGameFormModel
    {
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        public bool IsUserAlreadyJoin { get; set; }
    }
}
