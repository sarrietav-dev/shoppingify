namespace Shoppingify.Cart.Application.DTOs;

public class CartDto
{
    public string? Id { get; init; }
    public string Name { get; init; }
    public IEnumerable<CartItemDto> CartItems { get; init; }
    public DateTime CreatedAt { get; init; }
    public string State { get; init; }
    public string CartOwnerId { get; init; }

    public CartDto(Domain.Cart cart)
    {
        Id = cart.Id.ToString();
        Name = cart.Name;
        CartItems = cart.CartItems.Select(ci => new CartItemDto(ci));
        CreatedAt = cart.CreatedAt;
        State = cart.State.ToString();
        CartOwnerId = cart.CartOwnerId.ToString();
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
            CartOwnerId = new Domain.CartOwnerId(CartOwnerId)
        };

        return cart;
    }
}