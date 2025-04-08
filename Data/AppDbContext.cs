using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;

namespace EFCoreDemo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed initial data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Keyboard", Price = 49.99m },
            new Product { Id = 2, Name = "Monitor", Price = 199.99m }
        );
    }
}