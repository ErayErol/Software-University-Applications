namespace MessiFinder.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MessiFinderDbContext : IdentityDbContext
    {
        public MessiFinderDbContext(DbContextOptions<MessiFinderDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; init; }
        
        public virtual DbSet<Player> Players { get; init; }
        
        public virtual DbSet<Playground> Playgrounds { get; init; }
    }
}
