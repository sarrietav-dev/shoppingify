﻿namespace Shoppingify.Cart.Domain;

public record CartItem
{
    private readonly int _quantity;
    public required ProductId Product { get; init; }

    public required int Quantity
    {
        get => _quantity;
        init
        {
            if (value < 0)
                throw new InvalidOperationException("Quantity cannot be negative");

            _quantity = value;
        }
    }

    public CartItemStatus Status { get; init; } = CartItemStatus.Unchecked;
}