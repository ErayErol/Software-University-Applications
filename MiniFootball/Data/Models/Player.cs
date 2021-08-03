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

        // TODO : Creator of Game can add player to Game if player exist, is not exist can add anonymous player..... You can add search for Players by City, Name, Country, Age...... Players also have Picture and Skills Rating from 0 to 10
    }
}