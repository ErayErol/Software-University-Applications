namespace MessiFinder.Models.Playgrounds
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class PlaygroundCreateFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorEmptyValue)]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        public string Name { get; set; }

        [Required]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        public string Country { get; set; }

        [Required]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        public string Town { get; set; }

        [Required]
        [StringLength(DefaultMaxNameLength, MinimumLength = DefaultMinNameLength, ErrorMessage = ErrorRange)]
        public string Address { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = ErrorEmptyValue)]
        [StringLength(DefaultMaxDescription, MinimumLength = DefaultMinDescriptionLength,
            ErrorMessage = ErrorRange)]
        public string Description { get; set; }
    }
}
