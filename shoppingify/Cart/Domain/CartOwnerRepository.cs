namespace shoppingify.Cart.Domain;

public interface ICartOwnerRepository
{
    public Task<CartOwner?> Get(CartOwnerId id);
}