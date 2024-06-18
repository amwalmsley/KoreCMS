using Kore.CmsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Kore.CmsApi.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions options)  : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Customer> Customers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasData(GenerateCustomers().Take(30));
        }

        private static IEnumerable<Customer> GenerateCustomers()
        {
            int i = 0;

            while (++i < 1000)
            {
                yield return new Customer
                {
                    Id = i,
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    MiddleInitial = "M",
                    Title = $"Title{i}",
                    Email = $"email{i}@provider.com",
                    Phone = "5555555555",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
            }
        }
    }
}
