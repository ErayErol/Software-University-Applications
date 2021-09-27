namespace MiniFootball.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public DateTime Birthdate { get; set; }
        
        public override string PhoneNumber { get; set; }

        public string PhotoPath { get; set; }

        public virtual IEnumerable<UserGame> UserGames { get; init; } = new HashSet<UserGame>();
    }
}
