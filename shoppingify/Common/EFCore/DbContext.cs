using Microsoft.EntityFrameworkCore;
using Shoppingify.Cart.Domain;
using Product = Shoppingify.Products.Domain.Product;

namespace Shoppingify;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public required DbSet<CartOwner> CartOwners { get; set; }
    public required DbSet<Cart.Domain.Cart> Carts { get; set; }
    public required DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}