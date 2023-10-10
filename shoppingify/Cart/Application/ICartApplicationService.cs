using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application;

public interface ICartApplicationService
{
    public Task<CartId?> CreateCart(string cartOwnerId, string name);
    public Task<CartId?> CreateCart(string cartOwnerId, string name, IEnumerable<CartItem> cartItems);
    public Task<Domain.Cart?> GetActiveCart(string cartOwnerId);
    Task UpdateCartList(string ownerId, IEnumerable<CartItem> cartItems);
    Task CompleteCart(string cartOwnerId);
    Task CancelCart(string cartOwnerId);
    Task<IEnumerable<Domain.Cart>> GetCarts(string cartOwnerId);
}