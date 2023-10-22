using Shoppingify.Cart.Application;

namespace Shoppingify.Cart.Application.DTOs;

public record CartDto(string Id, string Name, IEnumerable<CartItemDto> CartItems, DateTime CreatedAt, string State, string CartOwnerId);