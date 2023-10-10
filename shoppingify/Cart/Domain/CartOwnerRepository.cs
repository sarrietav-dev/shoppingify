namespace Shoppingify.Cart.Domain;

public interface ICartOwnerRepository
{
    public Task<CartOwner?> Get(CartOwnerId id);
    public Task Add(CartOwner cartOwner);
}