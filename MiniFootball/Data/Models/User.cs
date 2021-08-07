﻿namespace MiniFootball.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static DataConstants.User;

    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public DateTime? Birthdate { get; set; }
        
        [Required]
        public override string PhoneNumber { get; set; }

        [Url]
        [Required]
        public string ImageUrl { get; set; }

        public virtual IEnumerable<UserGame> UserGames { get; init; } = new HashSet<UserGame>();
    }
}
