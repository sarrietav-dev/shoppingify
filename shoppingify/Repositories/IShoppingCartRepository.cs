using shoppingify.Entities;

namespace shoppingify.Repositories;

public interface IShoppingCartRepository
{
    public ShoppingCart GetCart(string id);
    public void SaveCart();
    public Task<ShoppingCart> CreateCart(ShoppingCart cart);
}