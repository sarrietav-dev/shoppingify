namespace shoppingify.Cart.Domain;

public interface ICartRepository
{
    public Task<Cart?> Get(CartId id);
    public Task Add(Cart cart);
}