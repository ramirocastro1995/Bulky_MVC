using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Basic configuration for entity framework
        //This is the db context with the tables
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }
        //Create a DbSet with the Category class,that we create,and migrate to create in db
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        //Default funtion of EF for populate the table with initial information
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    Price100= 80},
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
                    Price100 = 80
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
                    Price100 = 80
                }
                );
        }
    }
}
