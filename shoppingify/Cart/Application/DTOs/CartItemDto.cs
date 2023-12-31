﻿using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application.DTOs;

public record CartItemDto
{
    public required string ProductId { get; init; }
    public int Quantity { get; init; }
    public string? Status { get; init; } = CartItemStatus.Unchecked.ToString();

    public static CartItemDto ToCartItemDto(CartItem cartItem)
    {
        return new CartItemDto
        {
            ProductId = cartItem.Product.ToString(),
            Quantity = cartItem.Quantity,
            Status = cartItem.Status.ToString(),
        };
    }

    public CartItem ToCartItem()
    {
        var cartItem = new CartItem()
        {
            Product = new ProductId(Guid.Parse(ProductId)),
            Quantity = Quantity,
            Status = Status is not null ? Enum.Parse<CartItemStatus>(Status) : default
        };

        return cartItem;
    }
}