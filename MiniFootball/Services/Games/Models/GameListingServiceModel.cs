namespace MiniFootball.Services.Games.Models
{
    using Data.Models;
    using System;

    public class GameListingServiceModel
    {
        public string Id { get; set; }

        public Field Field { get; set; }

        public DateTime Date { get; set; }
    }
}
