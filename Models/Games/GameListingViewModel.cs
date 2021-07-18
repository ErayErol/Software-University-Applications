namespace MessiFinder.Models.Games
{
    using Data.Models;
    using System;

    public class GameListingViewModel
    {
        public int Id { get; set; }

        public Playground Playground { get; set; }

        public DateTime Date { get; set; }
    }
}
