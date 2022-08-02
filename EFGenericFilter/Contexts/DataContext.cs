using EFGenericFilter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFGenericFilter.Contexts;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var products = Enumerable.Range(1, 100).Select(x => new Product
        {
            Id = x,
            Name = $"Product{x}",
            Price = 100 + x,
            CreatedAt = DateTimeOffset.Now
        });
        
        modelBuilder.Entity<Product>().HasData(products);
        base.OnModelCreating(modelBuilder);
    }
}