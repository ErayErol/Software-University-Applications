namespace MiniFootball.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Models;

    public class MiniFootballDbContext : IdentityDbContext<User>
    {
        public MiniFootballDbContext(DbContextOptions<MiniFootballDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; init; }

        public virtual DbSet<Player> Players { get; init; }

        public virtual DbSet<Field> Fields { get; init; }

        public virtual DbSet<Admin> Admins { get; init; }
        
        public virtual DbSet<UserGame> UserGames { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Game>()
                .HasOne(g => g.Admin)
                .WithMany(a => a.Games)
                .HasForeignKey(g => g.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Admin>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserGame>().HasKey(table => new { table.UserId, table.GameId });

            base.OnModelCreating(builder);
        }
    }
}
