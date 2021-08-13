namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;
    using Services.Games.Models;

    public class CreateGameSecondStepViewModel : CreateGameLastStepViewModel
    {
        public IEnumerable<FieldListingServiceModel> Fields { get; set; }
    }
}
