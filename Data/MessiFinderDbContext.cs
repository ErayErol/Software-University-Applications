namespace MessiFinder.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class MessiFinderDbContext : IdentityDbContext
    {
        public MessiFinderDbContext(DbContextOptions<MessiFinderDbContext> options)
            : base(options)
        {
        }
    }
}
