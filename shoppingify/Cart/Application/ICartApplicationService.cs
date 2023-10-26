using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application;

public interface ICartApplicationService
{
    public Task<CartId?> CreateCart(string cartOwnerId, string name);
    public Task<CartId?> CreateCart(string cartOwnerId, string name, IEnumerable<CartItemDtoWithoutProduct> cartItems);
    public Task<CartDto?> GetActiveCart(string cartOwnerId);
    Task UpdateCartList(string ownerId, IEnumerable<CartItemDtoWithoutProduct> cartItems);
    Task CompleteCart(string cartOwnerId);
    Task CancelCart(string cartOwnerId);
    Task<IEnumerable<CartDto>> GetCarts(string cartOwnerId);
}