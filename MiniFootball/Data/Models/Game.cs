namespace MiniFootball.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Game;

    public class Game
    {
        [Key]
        public int Id { get; set; }

        public int PlaygroundId { get; set; }
        public virtual Playground Playground { get; set; }

        public DateTime Date { get; set; }

        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        public int NumberOfPlayers { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public int AdminId { get; set; }

        public virtual Admin Admin { get; set; }

        // you can add Stars(Rate) one hour after the match
    }
}
