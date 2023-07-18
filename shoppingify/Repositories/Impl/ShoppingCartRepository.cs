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
        var foundCart = _context.ShoppingCarts.Include(cart => cart.LineItems).ToList().Find(cart => cart.Id.ToString() == id) ?? throw new KeyNotFoundException();
        return foundCart;
    }

    public void SaveCart()
    {
        _context.SaveChangesAsync();
    }

    public ShoppingCart CreateCart(ShoppingCart cart)
    {
        var newCart = _context.ShoppingCarts.Add(cart);
        _context.SaveChangesAsync();
        return newCart.Entity;
    }
}