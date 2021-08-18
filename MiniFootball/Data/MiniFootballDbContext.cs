namespace MiniFootball.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MiniFootballDbContext : IdentityDbContext<User>
    {
        public MiniFootballDbContext(DbContextOptions<MiniFootballDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; init; }

        public virtual DbSet<Field> Fields { get; init; }
        
        public virtual DbSet<Country> Countries { get; init; }
        
        public virtual DbSet<City> Cities { get; init; }

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
                .Entity<Game>()
                .HasOne(g => g.Field)
                .WithMany(f => f.Games)
                .HasForeignKey(g => g.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Field>()
                .HasOne(f => f.Country)
                .WithMany(c => c.Fields)
                .HasForeignKey(g => g.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Field>()
                .HasOne(f => f.City)
                .WithMany(c => c.Fields)
                .HasForeignKey(g => g.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserGame>()
                .HasKey(table => new { table.UserId, table.GameId });

            base.OnModelCreating(builder);
        }
    }
}
