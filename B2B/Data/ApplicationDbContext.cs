using B2B.Models;
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
        public DbSet<Users> Users { get; set; }
        public DbSet<UserLogins> UserLogins { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<BankInfo> BankInfo { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<RemittanceDetail> RemittanceDetail { get; set; }


    }
}
