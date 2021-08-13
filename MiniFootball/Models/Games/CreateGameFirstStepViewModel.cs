namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;

    public class CreateGameFirstStepViewModel : CreateGameCountryAndCityViewModel
    {
        public IEnumerable<string> Countries { get; set; }
    }
}
