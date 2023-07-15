using shoppingify.Entities;

namespace shoppingify.Repositories.Impl;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ShoppingContext _context;

    public ShoppingCartRepository(ShoppingContext context)
    {
        _context = context;
    }

    public ShoppingCart GetCart(string id)
    {
        return _context.ShoppingCarts.Find(Guid.Parse(id)) ?? throw new KeyNotFoundException();
    }

    public void SaveCart()
    {
        _context.SaveChangesAsync();
    }

    public void CreateCart(ShoppingCart cart)
    {
        _context.ShoppingCarts.Add(cart);
        _context.SaveChangesAsync();
    }
}