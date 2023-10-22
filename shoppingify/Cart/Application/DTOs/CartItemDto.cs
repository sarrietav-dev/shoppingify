namespace Shoppingify.Cart.Application.DTOs;

public record CartItemDto(string ProductId, int Quantity, string? Status);