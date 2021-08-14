namespace MiniFootball.Services.Games.Models
{
    using System;

    public class GameDeleteServiceModel
    {
        public string GameId { get; set; }

        public string UserId { get; set; }

        public string FieldName { get; set; }

        public string FieldImageUrl { get; set; }
        
        public string FieldCountryName { get; set; }
        
        public string FieldCityName { get; set; }
        
        public DateTime Date { get; set; }

        public int Time { get; set; }
    }
}
