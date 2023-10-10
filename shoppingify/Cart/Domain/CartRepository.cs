namespace Shoppingify.Cart.Domain;

public interface ICartRepository
{
    public Task<Cart?> Get(CartId id);
    public Task<IEnumerable<Cart>> GetAll(string ownerId);
    public Task Add(Cart cart);
}