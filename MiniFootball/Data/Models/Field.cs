namespace MiniFootball.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Field;

    public class Field
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CountryMaxLength)]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Required]
        [MaxLength(CityMaxLength)]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public int AdminId { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual ICollection<Game> Games { get; init; } = new HashSet<Game>();

        // you can add Stars(Rate) only user that played here,
    }
}
