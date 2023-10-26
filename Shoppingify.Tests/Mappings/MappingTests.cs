using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

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

        var dto = CartDto.ToCartDto(cart);

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
            CreatedAt = DateTime.UtcNow,
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

        var cart = dto.ToCart();

        Assert.Equal(cart.Id.Value.ToString(), dto.Id);
        Assert.Equal(cart.Name, dto.Name);
        Assert.Equal(cart.CartOwnerId.Value, dto.CartOwnerId);
        Assert.Equal(cart.CartItems.Count, dto.CartItems.Count());
    }

    [Fact]
    public void Map_CartItemToDto_ShouldWork()
    {
        var cartItem = new CartItem()
        {
            Product = new ProductId(Guid.NewGuid()),
            Quantity = 1,
            Status = CartItemStatus.Unchecked
        };

        var dto = CartItemDto.ToCartItemDto(cartItem);

        Assert.Equal(cartItem.Product.Value.ToString(), dto.ProductId);
        Assert.Equal(cartItem.Quantity, dto.Quantity);
        Assert.Equal(cartItem.Status.ToString(), dto.Status);
    }

    [Fact]
    public void Map_DtoToCartItem_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var dto = new CartItemDto
        {
            ProductId = guid.ToString(),
            Quantity = 1,
            Status = CartItemStatus.Unchecked.ToString()
        };

        var cartItem = dto.ToCartItem();

        Assert.Equal(cartItem.Product.Value.ToString(), dto.ProductId);
        Assert.Equal(cartItem.Quantity, dto.Quantity);
        Assert.Equal(cartItem.Status.ToString(), dto.Status);
    }

    [Fact]
    public void Map_CartOwnerToDto_ShouldWork()
    {
        var cartOwner = new CartOwner
        {
            Id = new CartOwnerId(Guid.NewGuid().ToString()),
            ActiveCart = new CartId(Guid.NewGuid())
        };

        var dto = CartOwnerDto.ToCartOwnerDto(cartOwner);

        Assert.Equal(cartOwner.Id.Value, dto.Id);
        Assert.Equal(cartOwner.ActiveCart.ToString(), dto.ActiveCartId);
    }

    [Fact]
    public void Map_DtoToCartOwner_ShouldWork()
    {
        var guid = Guid.NewGuid();
        var dto = new CartOwnerDto
        {
            Id = guid.ToString(),
            ActiveCartId = Guid.NewGuid().ToString()
        };

        var cartOwner = dto.ToCartOwner();

        Assert.Equal(cartOwner.Id.Value, dto.Id);
        Assert.Equal(cartOwner.ActiveCart?.ToString(), dto.ActiveCartId);
    }
}