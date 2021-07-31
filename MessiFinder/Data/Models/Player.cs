namespace MessiFinder.Data.Models
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
    }
}