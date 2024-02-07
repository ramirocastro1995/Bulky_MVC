using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
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
        }
    }
}
