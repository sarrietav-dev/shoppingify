using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application.DTOs;

public class CartItemDto
{
    public string ProductId { get; init; }
    public int Quantity { get; init; }
    public string? Status { get; init; }

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