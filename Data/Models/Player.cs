namespace MessiFinder.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DefaultMaxNameLength)]
        public string Name { get; set; }
    }
}