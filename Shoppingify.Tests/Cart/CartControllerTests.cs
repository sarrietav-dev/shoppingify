using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using shoppingify.Cart.Application;
using shoppingify.Cart.Domain;
using shoppingify.Cart.Infrastructure.Controllers;
using shoppingify.IAM.Application;

namespace Shoppingify.Tests.Cart;

public class CartControllerTests
{
    private readonly Mock<ICartApplicationService> _cartApplicationServiceMock;
    private readonly CartController _cartController;
    private readonly Mock<IAuthenticationProviderService> _authenticationProviderServiceMock;
    private readonly Faker<shoppingify.Cart.Domain.Cart> _cartFaker;
    private readonly Faker<CartItem> _cartItemFaker;

    public CartControllerTests()
    {
        _cartApplicationServiceMock = new Mock<ICartApplicationService>();
        _authenticationProviderServiceMock = new Mock<IAuthenticationProviderService>();
        _cartController = new CartController(_cartApplicationServiceMock.Object,
            _authenticationProviderServiceMock.Object);
        _cartFaker = new Faker<shoppingify.Cart.Domain.Cart>()
            .RuleFor(x => x.Id, f => new CartId(f.Random.Guid()))
            .RuleFor(x => x.Name, f => f.Random.String2(10))
            .RuleFor(x => x.CartOwnerId, f => new CartOwnerId(f.Random.Guid().ToString()))
            .RuleFor(x => x.CartItems, f => f.Make(3, () => new CartItem
            {
                Product = new Product(f.Random.Guid(), f.Commerce.Product()),
                Quantity = f.Random.Int(1, 10),
                Status = f.PickRandom<CartItemStatus>()
            }));

        _cartItemFaker = new Faker<CartItem>()
            .RuleFor(x => x.Product, f => new Product(f.Random.Guid(), f.Commerce.Product()))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(x => x.Status, f => f.PickRandom<CartItemStatus>());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public async Task GetCarts_ReturnsOk(int cartCount)
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var carts = _cartFaker.Generate(cartCount);
        _cartApplicationServiceMock.Setup(x => x.GetCarts(cartOwnerId)).ReturnsAsync(carts);

        // Act
        var result = await _cartController.GetCarts(cartOwnerId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(carts, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetActiveCart_WithCart_ReturnsOk()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var cart = _cartFaker.Generate();
        _cartApplicationServiceMock.Setup(x => x.GetActiveCart(cartOwnerId)).ReturnsAsync(cart);

        // Act
        var result = await _cartController.GetActiveCart(cartOwnerId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(cart, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetActiveCart_WithoutCart_ReturnsNoContent()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        _cartApplicationServiceMock.Setup(x => x.GetActiveCart(cartOwnerId)).ReturnsAsync((shoppingify.Cart.Domain.Cart?)null);

        // Act
        var result = await _cartController.GetActiveCart(cartOwnerId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CreateCart_WithValidData_ReturnsCreated()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var cartId = Guid.NewGuid();
        var name = "My cart";
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.CreateCart(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<CartItem>>())).ReturnsAsync(new CartId(cartId));

        // Act
        var result = await _cartController.CreateCart(cart: new CreateCartCommand(name, items), cartOwnerId);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CreateCart_WithActiveCart_ReturnsBadRequest()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var name = "My cart";
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.CreateCart(cartOwnerId, name, items)).Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.CreateCart(cart: new CreateCartCommand(name, items), cartOwnerId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCart_ReturnsNull_ReturnsServerError()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var name = "My cart";
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.CreateCart(cartOwnerId, name, items)).ReturnsAsync((CartId?)null);

        // Act
        var result = await _cartController.CreateCart(cart: new CreateCartCommand(name, items), cartOwnerId);

        // Assert
        Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, ((StatusCodeResult)result).StatusCode);
    }

    [Fact]
    public async Task UpdateCartList_WithValidData_ReturnsOk()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var items = _cartItemFaker.Generate(3);

        // Act
        var result = await _cartController.UpdateCartList(cartItems: items, cartOwnerId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateCartList_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.UpdateCartList(cartOwnerId, items)).Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.UpdateCartList(cartItems: items, cartOwnerId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CompleteCart_WithValidData_ReturnsOk()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var items = _cartItemFaker.Generate(3);

        // Act
        var result = await _cartController.CompleteCart(cartOwnerId, items);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CompleteCart_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.CompleteCart(cartOwnerId)).Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.CompleteCart(cartOwnerId, items);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CancelCart_WithValidData_ReturnsOk()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();

        // Act
        var result = await _cartController.CancelCart(cartOwnerId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CancelCart_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange
        var cartOwnerId = Guid.NewGuid().ToString();
        _cartApplicationServiceMock.Setup(x => x.CancelCart(cartOwnerId)).Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.CancelCart(cartOwnerId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}