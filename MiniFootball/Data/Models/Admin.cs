namespace MiniFootball.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Admin;

    public class Admin
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IEnumerable<Game> Games { get; init; } = new List<Game>();

        public virtual IEnumerable<Field> Fields { get; init; } = new List<Field>();
    }
}