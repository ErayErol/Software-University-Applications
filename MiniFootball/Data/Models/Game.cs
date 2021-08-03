﻿namespace MiniFootball.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Game;

    public class Game
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public int FieldId { get; set; }
        public virtual Field Field { get; set; }

        public DateTime Date { get; set; }

        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        public int NumberOfPlayers { get; set; }

        public bool Goalkeeper { get; set; }

        public bool Ball { get; set; }

        public bool Jerseys { get; set; }

        [Range(NumberOfPlayersMin, NumberOfPlayersMax)]
        public int Places { get; set; }

        public bool HasPlaces { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public int AdminId { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual IEnumerable<UserGame> UserGames { get; init; } = new HashSet<UserGame>();

        // you can add Stars(Rate) a hour after the match
    }
}