using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application.DTOs;

public record CartItemDtoWithoutProduct : CartItemDto
{
    public required string ProductId { get; init; }

    public static CartItemDtoWithoutProduct ToCartItemDto(CartItem cartItem)
    {
        return new CartItemDtoWithoutProduct
        {
            ProductId = cartItem.Product.ToString(),
            Quantity = cartItem.Quantity,
            Status = cartItem.Status.ToString(),
        };
    }

    public override CartItem ToCartItem()
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