using Microsoft.EntityFrameworkCore;
using TTechEcommerce.DAL.Extensions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace EcommerceApi.Entities
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext()
        {

        }

        public EcommerceContext(DbContextOptions<EcommerceContext>  options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public DbSet<Product> Products { get; set;}

        public DbSet<Order> Orders { get; set;}

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set;} 
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
