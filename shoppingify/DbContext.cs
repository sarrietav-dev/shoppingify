using Microsoft.EntityFrameworkCore;

namespace shoppingify;

internal class ShoppingContext : DbContext
{
    public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
    {
    }
}