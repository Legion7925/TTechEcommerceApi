using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace TTechEcommerce.DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedProducts(modelBuilder);
        }

        public static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData
            (
               new Category {Id = 1, Name = "Book", Description = "Books" },
               new Category {Id = 2, Name = "Phone", Description = "Phones" },
               new Category {Id = 3, Name = "Laptop", Description = "Laptops" },
               new Category {Id = 4, Name = "Computer", Description = "Computers" },
               new Category {Id = 5, Name = "Smart Phone", Description = "Smart Phones" }
            );
        }

        public static void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData
            (
               new Product {Id = 1, Name = "Meaning Of Life", Color = "Black" , CategoryId = 1 , Price = 29.99 , IsAvailable = true },
               new Product {Id = 2, Name = "Panasonic", Color = "White" , CategoryId = 2 , Price = 64.99 , IsAvailable = false},
               new Product {Id = 3, Name = "Asus K556UF", Color = "Black" , CategoryId = 3 , Price = 399.99 , IsAvailable = true },
               new Product {Id = 4, Name = "MSI Gaming PC", Color = "White And Black" , CategoryId = 4 , Price = 2000 , IsAvailable = true },
               new Product {Id = 5, Name = "Iphone 13 ProMax", Color = "Blue" , CategoryId = 5 , Price = 1999 , IsAvailable = false }
            );
        }
    }
}
