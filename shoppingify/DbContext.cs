using Microsoft.EntityFrameworkCore;
using shoppingify.Cart.Domain;

namespace shoppingify;

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