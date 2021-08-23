namespace MiniFootball.Models.Fields
{
    using Data;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class FieldEditFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Empty)]
        [StringLength(Field.NameMaxLength, MinimumLength = Field.NameMinLength, ErrorMessage = ErrorMessages.Range)]
        public string Name { get; set; }

        [Required]
        [MaxLength(Field.PhoneNumberMaxLength)]
        [Display(Name = DataConstants.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(Field.AddressMaxLength, MinimumLength = Field.AddressMinLength, ErrorMessage = ErrorMessages.Range)]
        public string Address { get; set; }

        [Required]
        [Display(Name = DataConstants.ImageUrl)]
        [Url(ErrorMessage = ErrorMessages.Url)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = ErrorMessages.Empty)]
        [StringLength(Field.DescriptionMaxLength, MinimumLength = Field.DescriptionMinLength, ErrorMessage = ErrorMessages.Range)]
        public string Description { get; set; }

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }
    }
}