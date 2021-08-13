namespace MiniFootball.Models.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Services.Games.Models;

    public class CreateGameSecondStepViewModel : CreateGameLastStepViewModel
    {
        public IEnumerable<FieldListingServiceModel> Fields { get; set; }
    }

    public class CreateGameLastStepViewModel : CreateGameCountryAndCityViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Select field:")]
        public int FieldId { get; set; }
    }
}
