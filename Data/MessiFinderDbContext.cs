namespace MessiFinder.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MessiFinderDbContext : IdentityDbContext<User>
    {
        public MessiFinderDbContext(DbContextOptions<MessiFinderDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; init; }

        public virtual DbSet<Player> Players { get; init; }

        public virtual DbSet<Playground> Playgrounds { get; init; }

        public virtual DbSet<Admin> Admins { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Game>()
                .HasOne(g => g.Admin)
                .WithMany(a => a.Games)
                .HasForeignKey(g => g.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Playground>()
                .HasOne(p => p.Admin)
                .WithMany(a => a.Playgrounds)
                .HasForeignKey(p => p.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Admin>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
