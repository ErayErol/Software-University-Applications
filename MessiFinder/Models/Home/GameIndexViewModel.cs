namespace MessiFinder.Models.Home
{
    using System;
    using Data.Models;

    public class GameIndexViewModel
    {
        public int Id { get; set; }

        public Playground Playground { get; set; }

        public DateTime Date { get; set; }
    }
}
