using ALM_Tenta.Models;
using Microsoft.EntityFrameworkCore;

namespace ALM_Tenta.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);
        }
    }
}
