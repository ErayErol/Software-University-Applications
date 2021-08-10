namespace MiniFootball.Services.Fields
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    public class FieldDetailServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }
        public Country Country { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }
        public City City { get; set; }

        public string Address { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } 

        public bool Parking { get; set; }

        public bool Shower { get; set; }

        [Display(Name = "Changing Room")]
        public bool ChangingRoom { get; set; }

        public bool Cafe { get; set; }

        public string Description { get; set; }
    }
}