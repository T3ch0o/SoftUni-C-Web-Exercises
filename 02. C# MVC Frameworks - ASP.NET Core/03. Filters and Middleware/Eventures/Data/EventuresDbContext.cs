namespace Eventures.Data
{
    using Eventures.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class EventuresDbContext : IdentityDbContext<ApplicationUser>
    {
        public EventuresDbContext(DbContextOptions<EventuresDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<CustomLog> Logs { get; set; }
    }
}
