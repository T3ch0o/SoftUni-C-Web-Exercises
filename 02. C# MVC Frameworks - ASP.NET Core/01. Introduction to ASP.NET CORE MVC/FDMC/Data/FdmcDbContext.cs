namespace FDMC.Data
{
    using FDMC.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FdmcDbContext : IdentityDbContext<User>
    {
        public FdmcDbContext(DbContextOptions<FdmcDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
