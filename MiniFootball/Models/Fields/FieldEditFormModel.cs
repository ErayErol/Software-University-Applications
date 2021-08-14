namespace MiniFootball.Models.Fields
{
    using System.ComponentModel.DataAnnotations;
    using Data;

    public class FieldEditFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        [StringLength(DataConstants.Field.NameMaxLength, MinimumLength = DataConstants.Field.NameMinLength, ErrorMessage = DataConstants.ErrorMessages.Range)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataConstants.Field.PhoneNumberMaxLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(DataConstants.Field.AddressMaxLength, MinimumLength = DataConstants.Field.AddressMinLength, ErrorMessage = DataConstants.ErrorMessages.Range)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        [Url(ErrorMessage = DataConstants.ErrorMessages.Url)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = DataConstants.ErrorMessages.Empty)]
        [StringLength(DataConstants.Field.DescriptionMaxLength, MinimumLength = DataConstants.Field.DescriptionMinLength, ErrorMessage = DataConstants.ErrorMessages.Range)]
        public string Description { get; set; }

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }
    }
}