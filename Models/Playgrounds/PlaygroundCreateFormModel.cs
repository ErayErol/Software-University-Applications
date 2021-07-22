namespace MessiFinder.Models.Playgrounds
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Playground;

    public class PlaygroundCreateFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = Range)]
        public string Name { get; set; }

        [Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = Range)]
        public string Country { get; set; }

        public IEnumerable<string> Countries { get; set; }

        [Required]
        [StringLength(TownMaxLength, MinimumLength = TownMinLength, ErrorMessage = Range)]
        public string Town { get; set; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = Range)]
        public string Address { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }
    }
}
