using Shoppingify.Cart.Domain;
using Shoppingify.Products.Application.Dtos;
using Shoppingify.Products.Domain;
using ProductId = Shoppingify.Cart.Domain.ProductId;

namespace Shoppingify.Cart.Application.DTOs;

public record CartItemDtoWithProduct : CartItemDto
{
    public required ProductDto Product { get; init; }

    public static CartItemDtoWithProduct ToCartItemDto(CartItem cartItem, Product product)
    {
        return new CartItemDtoWithProduct
        {
            Product = ProductDto.FromProduct(product),
            Quantity = cartItem.Quantity,
            Status = cartItem.Status.ToString(),
        };
    }

    public override CartItem ToCartItem()
    {
        var cartItem = new CartItem()
        {
            Product = new ProductId(Guid.Parse(Product.Id)),
            Quantity = Quantity,
            Status = Status is not null ? Enum.Parse<CartItemStatus>(Status) : default
        };

        return cartItem;
    }
};