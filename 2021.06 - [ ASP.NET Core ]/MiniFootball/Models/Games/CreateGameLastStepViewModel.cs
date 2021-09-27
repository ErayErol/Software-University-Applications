namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.Field;

    public class CreateGameLastStepViewModel : CreateGameCountryAndCityViewModel
    {
        public string Name { get; set; }

        [Display(Name = SelectField)]
        public int FieldId { get; set; }
    }
}