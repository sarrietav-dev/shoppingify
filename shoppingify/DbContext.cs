using Microsoft.EntityFrameworkCore;
using Shoppingify.Cart.Domain;

namespace Shoppingify;

public class AppDbContext : DbContext
{
    public DbSet<CartOwner> CartOwners { get; set; }
    public DbSet<Cart.Domain.Cart> Carts { get; set; }
    public DbSet<Products.Domain.Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}