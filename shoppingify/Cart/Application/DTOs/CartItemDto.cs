using System.Text.Json.Serialization;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application.DTOs;

[JsonDerivedType(typeof(CartItemDtoWithProduct))]
[JsonDerivedType(typeof(CartItemDtoWithoutProduct))]
public abstract record CartItemDto
{
    public int Quantity { get; init; }
    public string? Status { get; init; } = CartItemStatus.Unchecked.ToString();

    public abstract CartItem ToCartItem();
};