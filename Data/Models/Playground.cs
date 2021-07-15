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
        [MaxLength(DefaultMaxNameLength)]
        public string Country { get; set; }

        [Required]
        [MaxLength(DefaultMaxNameLength)]
        public string Town { get; set; }

        [Required]
        [MaxLength(DefaultMaxDescription)]
        public string Address { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(DefaultMaxDescription)]
        public string Description { get; set; }

        // you can add Stars(Rate), For reservations, Parking, Shower, Ball, Shirts, Changing room, Cafe
    }
}
