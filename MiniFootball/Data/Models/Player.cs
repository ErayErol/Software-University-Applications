namespace MiniFootball.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Player;

    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        // TODO: You can add search for Players by City, Name, Country, Age...... Add Picture, Skills Rating from 0 to 10
    }
}