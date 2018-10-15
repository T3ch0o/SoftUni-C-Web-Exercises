namespace IRunes.Data
{
    using IRunes.Models;

    using Microsoft.EntityFrameworkCore;

    internal class IRunesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.; Database=IRunes; Integrated Security=true")
                          .UseLazyLoadingProxies();
        }
    }
}