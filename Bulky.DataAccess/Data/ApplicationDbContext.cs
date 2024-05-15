using Bulky.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        //Basic configuration for entity framework
        //This is the db context with the tables
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }
        //Create a DbSet with the Category class,that we create,and migrate to create in db
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        //Default funtion of EF for populate the table with initial information
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Using the modelBuilder,we select the class/table that we want to populate
            //Use .HasData to populate the table
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }

                ) ;
            modelBuilder.Entity<Product>().HasData(

                new Product 
                { 
                    Id = 1,
                    Title = "Test", 
                    Description = "test", 
                    ISBN = "SADASDASF", 
                    Author = "Edgar" ,
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100= 80,
                    CategoryId = 1
                },
                
                new Product
                {
                    Id = 2,
                    Title = "Test2",
                    Description = "test",
                    ISBN = "SADASDASF",
                    Author = "Edgar",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 2

                },
                new Product
                {
                    Id = 3,
                    Title = "Test3",
                    Description = "test",
                    ISBN = "SADASDASF",
                    Author = "Edgar",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 3
                }
                );
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Company Test",
                    StreetAddress = "Fake Street 1",
                    City = "Fake City",
                    State = "Fake State",
                    PostalCode = "ABC123",
                    PhoneNumber = 12345
                }

                );
        }
    }
}
