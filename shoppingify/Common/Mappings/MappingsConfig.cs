using Mapster;
using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Common.Mappings;

public class MappingsConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cart.Domain.Cart, CartDto>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CartItems, src => src.CartItems)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.State, src => src.State.ToString())
            .Map(dest => dest.CartOwnerId, src => src.CartOwnerId.ToString());

        config.NewConfig<CartDto, Cart.Domain.Cart>()
            .Map(dest => dest.Id, src => new CartId(Guid.Parse(src.Id)))
            .Map(dest => dest.CartOwnerId, src => new CartOwnerId(src.CartOwnerId))
            .Map(dest => dest.State, src => Enum.Parse<CartState>(src.State));

        config.NewConfig<CartItem, CartItemDto>()
            .Map(dest => dest.ProductId, src => src.Product.ToString())
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Status, src => src.Status.ToString());

        config.NewConfig<CartItemDto, CartItem>()
            .TwoWays()
            .Map(dest => dest.Product, src => new ProductId(Guid.Parse(src.ProductId)))
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Status,
                src => Enum.Parse<CartItemStatus>(src.Status ?? CartItemStatus.Unchecked.ToString()));

        config.NewConfig<CartOwner, CartOwnerDto>()
            .TwoWays()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.ActiveCartId, src => src.ActiveCart.ToString());

    }
}