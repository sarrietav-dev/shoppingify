using Microsoft.EntityFrameworkCore;
using shoppingify.Entities;

namespace shoppingify;

public class ShoppingContext : DbContext
{
    public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
    {
    }
    
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<LineItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingCart>(shoppingCart =>
        {
            shoppingCart.HasKey("Id");
            shoppingCart.Property(sc => sc.Name).IsRequired();
            shoppingCart.HasMany(sc => sc.LineItems).WithOne().IsRequired();
        });
        
        modelBuilder.Entity<Product>(product =>
        {
            product.HasKey("Id");
            product.Property(p => p.Name).IsRequired();
            product.Property(p => p.Note).IsRequired();
            product.Property(p => p.Image).IsRequired();
        });
        
        modelBuilder.Entity<LineItem>(lineItem =>
        {
            lineItem.HasKey("Id");
            lineItem.Property(li => li.Quantity).IsRequired();
            lineItem.Property(li => li.IsChecked).HasDefaultValue(false);
            lineItem.HasOne<Product>(li => li.Product).WithMany().IsRequired();
        });
    }
}