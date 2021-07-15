namespace MessiFinder.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Game
    {
        [Key]
        public int Id { get; set; }

        public int PlaygroundId { get; set; }
        public virtual Playground Playground { get; set; }

        public DateTime Date { get; set; }

        [Range(PlaygroundMinNumberOfPlayers, PlaygroundMaxNumberOfPlayers)]
        public int NumberOfPlayers { get; set; }

        public bool WithGoalkeeper { get; set; }

        [Required]
        [MaxLength(DefaultMaxDescription)]
        public string Description { get; set; }

        // you can add Stars(Rate after the match), Number(of Owner), Ball, Shirts(which team what color to pick)
    }
}
