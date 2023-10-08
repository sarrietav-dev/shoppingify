using shoppingify.Cart.Domain;

namespace shoppingify.Cart.Application;

public interface ICartApplicationService
{

    public Task CreateCart(string cartOwnerId, string name);
    public Task<Domain.Cart?> GetActiveCart(string cartOwnerId);
    Task UpdateCartList(string ownerId, IEnumerable<CartItem> cartItems);
    Task CompleteCart(string cartOwnerId);
    Task CancelCart(string cartOwnerId);
    Task<IEnumerable<Domain.Cart>> GetCarts(string cartOwnerId);
}