namespace shoppingify.Cart.Domain;

public interface ICartRepository
{
    public Task<Cart?> Get(CartId id);
}