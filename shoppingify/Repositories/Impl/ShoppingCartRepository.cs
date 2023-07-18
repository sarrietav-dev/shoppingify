using Microsoft.EntityFrameworkCore;
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
        // Find the cart with the given id and populate its line items
        return _context.ShoppingCarts
            .Include(sc => sc.LineItems)
            .ThenInclude(li => li.Product).ToList()
            .FirstOrDefault(sc => sc.Id.ToString() == id) ?? throw new KeyNotFoundException();
    }

    public void SaveCart()
    {
        _context.SaveChangesAsync();
    }

    public async Task<ShoppingCart> CreateCart(ShoppingCart cart)
    {
        var newCart = _context.ShoppingCarts.Add(cart);
        await _context.SaveChangesAsync();
        return newCart.Entity;
    }
}