using AutoMapper;
using shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;
using Shoppingify.Products.Domain;
using ProductId = Shoppingify.Cart.Domain.ProductId;

namespace Shoppingify.Lib;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Cart.Domain.Cart, CartDto>()
            .ForCtorParam(nameof(CartDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(CartDto.State),
                opt => opt.MapFrom(src => src.State.ToString()))
            .ReverseMap();

        CreateMap<CartItem, CartItemDto>()
            .ForCtorParam(nameof(CartItemDto.Quantity),
                opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(CartItemDto.Status),
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(CartItemDto.ProductId),
                opt => opt.MapFrom(src => src.Product))
            .ReverseMap()
            .ForAllMembers(opt => opt.Ignore());

        CreateMap<CartOwner, CartOwnerDto>()
            .ForCtorParam(nameof(CartOwnerDto.ActiveCartId),
                opt => opt.MapFrom(src => src.ActiveCart)).ReverseMap();

        CreateMap<CartId, string>()
            .ConstructUsing(src => src.Value.ToString()).ReverseMap();

        CreateMap<CartOwnerId, string>()
            .ConstructUsing(src => src.Value).ReverseMap();

        CreateMap<ProductId, string>()
            .ForCtorParam(nameof(ProductId.Value), opt => opt.MapFrom(src => src.Value.ToString()));
    }
}