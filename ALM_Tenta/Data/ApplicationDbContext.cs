using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .HasMany(a => a.SenderTransactions)
                .WithOne(s => s.Sender)
                .HasForeignKey(s => s.SenderId);

            builder.Entity<Account>()
                .HasMany(a => a.RecipentTransactions)
                .WithOne(s => s.Recipient)
                .HasForeignKey(s => s.RecipentId);
        }
    }
}
