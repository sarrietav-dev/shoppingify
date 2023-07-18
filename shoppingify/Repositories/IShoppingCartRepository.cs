using shoppingify.Entities;

namespace shoppingify.Repositories;

public interface IShoppingCartRepository
{
    public ShoppingCart GetCart(string id);
    public Task SaveCart();
    public Task<ShoppingCart> CreateCart(ShoppingCart cart);
}