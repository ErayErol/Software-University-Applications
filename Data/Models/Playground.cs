namespace MessiFinder.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Playground
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DefaultMaxNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DefaultMaxDescription)]
        public string Description { get; set; }
    }
}
