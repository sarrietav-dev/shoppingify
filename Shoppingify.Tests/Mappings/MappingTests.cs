using Mapster;
using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;
using Shoppingify.Common.Mappings;

namespace Shoppingify.Tests.Mappings;

public class MappingTests
{
    [Fact]
    public void Map_CartToDto_ShouldWork()
    {
        var cart = new Shoppingify.Cart.Domain.Cart()
        {
            Id = new CartId(Guid.NewGuid()),
            Name = "Test Cart",
            CartOwnerId = new CartOwnerId("hey"),
            CartItems =
            {
                new CartItem()
                {
                    Product = new ProductId(Guid.NewGuid()),
                    Quantity = 1,
                    Status = CartItemStatus.Unchecked
                },
                new CartItem()
                {
                    Product = new ProductId(Guid.NewGuid()),
                    Quantity = 4,
                    Status = CartItemStatus.Checked
                }
            }
        };

        var dto = cart.Adapt<CartDto>();

        Assert.Equal(cart.Id.Value.ToString(), dto.Id);
        Assert.Equal(cart.Name, dto.Name);
        Assert.Equal(cart.CartOwnerId.Value, dto.CartOwnerId);
        Assert.Equal(cart.CartItems.Count, dto.CartItems.Count());
    }

    [Fact]
    public void Map_DtoToCart_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var dto = new CartDto
        {
            Id = guid.ToString(),
            Name = "Hey",
            State = CartState.Active.ToString(),
            CartOwnerId = "id",
            CartItems = new List<CartItemDto>
            {
                new()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    Status = CartItemStatus.Unchecked.ToString()
                },
                new()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 4,
                    Status = CartItemStatus.Checked.ToString()
                }
            }
        };

        var cart = dto.Adapt<Shoppingify.Cart.Domain.Cart>();

        Assert.Equal(cart.Id.Value.ToString(), dto.Id);
        Assert.Equal(cart.Name, dto.Name);
        Assert.Equal(cart.CartOwnerId.Value, dto.CartOwnerId);
        Assert.Equal(cart.CartItems.Count, dto.CartItems.Count());
    }

    [Fact]
    public void Map_CartIdToString_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var cartId = new CartId(guid);

        var str = cartId.Adapt<string>();

        Assert.Equal(guid.ToString(), str);
    }

    [Fact]
    public void Map_CartOwnerIdToString_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var cartOwnerId = new CartOwnerId(guid.ToString());

        var str = cartOwnerId.Adapt<string>();

        Assert.Equal(guid.ToString(), str);
    }

    [Fact]
    public void Map_ProductIdToString_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var productId = new ProductId(guid);

        var str = productId.Adapt<string>();

        Assert.Equal(guid.ToString(), str);
    }

    [Fact]
    public void Map_StringToCartId_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var str = guid.ToString();

        var cartId = str.Adapt<CartId>();

        Assert.Equal(guid, cartId.Value);
    }

    [Fact]
    public void Map_StringToCartOwnerId_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var str = guid.ToString();

        var cartOwnerId = str.Adapt<CartOwnerId>();

        Assert.Equal(guid.ToString(), cartOwnerId.Value);
    }

    [Fact]
    public void Map_StringToProductId_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var str = guid.ToString();

        var productId = str.Adapt<ProductId>();

        Assert.Equal(guid, productId.Value);
    }
}