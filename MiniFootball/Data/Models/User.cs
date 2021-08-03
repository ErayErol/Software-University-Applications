namespace MiniFootball.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static DataConstants.User;

    public class User : IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        public virtual IEnumerable<UserGame> UserGames { get; init; } = new HashSet<UserGame>();
    }
}
