namespace MiniFootball.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    public class CreateGameLastStepViewModel : CreateGameCountryAndCityViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Select field:")]
        public int FieldId { get; set; }
    }
}