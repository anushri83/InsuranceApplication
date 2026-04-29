using Microsoft.EntityFrameworkCore; // <--- This fixes DbSet and ModelBuilder
using Insurance.Domain.Models;      // <--- This fixes Policy, Claim, User, etc.

namespace Insurance.Infrastructure.Data
{
public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Your 4 Tables
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CustomerPolicy> CustomerPolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // This is where you can define "Fluent API" rules (like unique keys)
        }
    }
}