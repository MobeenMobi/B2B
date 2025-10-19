using Microsoft.EntityFrameworkCore;

namespace B2B.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // Example table
        public DbSet<User> Users { get; set; }
    }
}
