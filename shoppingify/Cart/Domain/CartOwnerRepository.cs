namespace Shoppingify.Cart.Domain;

public interface ICartOwnerRepository
{
    public CartOwner? Get(CartOwnerId id);
}