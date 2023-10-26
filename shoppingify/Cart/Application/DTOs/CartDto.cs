using System.Text.Json.Serialization;

namespace Shoppingify.Cart.Application.DTOs;

public record CartDto
{
    public string? Id { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<CartItemDto> CartItems { get; init; }
    public DateTime CreatedAt { get; init; }
    public required string State { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string? CartOwnerId { get; init; }

    public static CartDto ToCartDto(Domain.Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id.ToString(),
            Name = cart.Name,
            CartItems = cart.CartItems.Select(CartItemDtoWithoutProduct.ToCartItemDto),
            CreatedAt = cart.CreatedAt,
            State = cart.State.ToString(),
            CartOwnerId = cart.CartOwnerId.ToString(),
        };
    }

    public static CartDto ToCartDto(Domain.Cart cart, IEnumerable<CartItemDto> cartItems)
    {
        return new CartDto
        {
            Id = cart.Id.ToString(),
            Name = cart.Name,
            CartItems = cartItems,
            CreatedAt = cart.CreatedAt,
            State = cart.State.ToString(),
            CartOwnerId = cart.CartOwnerId.ToString(),
        };
    }

    public Domain.Cart ToCart()
    {
        var cart = new Domain.Cart
        {
            Id = new Domain.CartId(Guid.Parse(Id ?? Guid.Empty.ToString())),
            Name = Name,
            CartItems = CartItems.Select(ci => ci.ToCartItem()).ToList(),
            CreatedAt = CreatedAt,
            State = Enum.Parse<Domain.CartState>(State),
            CartOwnerId = new Domain.CartOwnerId(CartOwnerId ?? throw new InvalidOperationException() )
        };

        return cart;
    }
}