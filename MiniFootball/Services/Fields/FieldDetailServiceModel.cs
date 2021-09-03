namespace MiniFootball.Services.Fields
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.Field;

    public class FieldDetailServiceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = Range)]
        public string Name { get; set; }

        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = Range)]
        [Display(Name = Country.CountryName)]
        public string CountryName { get; set; }

        [StringLength(CityMaxLength, MinimumLength = CityMinLength, ErrorMessage = Range)]
        [Display(Name = City.CityName)]
        public string CityName { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        [Display(Name = DataConstants.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength, ErrorMessage = Range)]
        public string Address { get; set; }

        [Required]
        [Display(Name = DataConstants.ImageUrl)]
        [Url(ErrorMessage = Url)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = Empty)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = Range)]
        public string Description { get; set; }

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        [Display(Name = DataConstants.ChangingRoom)]
        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }
    }
}