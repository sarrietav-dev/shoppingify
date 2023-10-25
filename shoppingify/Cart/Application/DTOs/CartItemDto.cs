namespace Shoppingify.Cart.Application.DTOs;

public class CartItemDto
{
    public required string ProductId { get; init; }
    public int Quantity { get; init; }
    public string? Status { get; init; }
}