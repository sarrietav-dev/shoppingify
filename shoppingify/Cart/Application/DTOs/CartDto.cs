namespace Shoppingify.Cart.Application.DTOs;

public class CartDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<CartItemDto> CartItems { get; init; }
    public DateTime CreatedAt { get; init; }
    public required string State { get; init; }
    public required string CartOwnerId { get; init; }
}