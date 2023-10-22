using Mapster;
using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Common.Mappings;

public class MappingsConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cart.Domain.Cart, CartDto>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CartItems, src => src.CartItems)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.State, src => src.State.ToString())
            .Map(dest => dest.CartOwnerId, src => src.CartOwnerId.Value.ToString())
            .TwoWays();

        config.NewConfig<Cart.Domain.CartItem, CartItemDto>()
            .Map(dest => dest.ProductId, src => src.Product.Value.ToString())
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .TwoWays();

        config.NewConfig<CartItemDto, CartItem>()
            .Map(dest => dest.Product, src => new ProductId(Guid.Parse(src.ProductId)))
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Status,
                src => Enum.Parse<CartItemStatus>(src.Status ?? CartItemStatus.Unchecked.ToString()))
            .TwoWays();

        config.NewConfig<Cart.Domain.CartOwner, CartOwnerDto>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.ActiveCartId, src => src.ActiveCart.ToString())
            .TwoWays();

        config.NewConfig<CartId, string>()
            .Map(dest => dest, src => src.ToString())
            .TwoWays();

        config.NewConfig<CartOwnerId, string>()
            .Map(dest => dest, src => src.ToString())
            .TwoWays();

        config.NewConfig<ProductId, string>()
            .Map(dest => dest, src => src.ToString())
            .TwoWays();
    }
}