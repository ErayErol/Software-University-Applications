namespace MessiFinder.Models.Games
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;

    public class CreateGameFormModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Playground")]
        public int PlaygroundId { get; set; }

        public IEnumerable<GamePlaygroundViewModel> Playgrounds { get; set; }

        //public DateTime StartTime { get; set; }

        //public DateTime EndTime { get; set; }

        //public int Players { get; set; }

        //public string Description { get; set; }

        //public decimal Price { get; set; }

        //public bool WithGoalKeeper { get; set; }
    }
}
