using System.Security.Claims;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shoppingify.Cart.Application;
using Shoppingify.Cart.Domain;
using Shoppingify.Cart.Infrastructure.Controllers;

namespace Shoppingify.Tests.Cart;

public class CartControllerTests
{
    private readonly Mock<ICartApplicationService> _cartApplicationServiceMock;
    private readonly CartController _cartController;
    private readonly Faker<Shoppingify.Cart.Domain.Cart> _cartFaker;
    private readonly Faker<CartItem> _cartItemFaker;

    public CartControllerTests()
    {
        _cartApplicationServiceMock = new Mock<ICartApplicationService>();
        _cartController = new CartController(_cartApplicationServiceMock.Object);
        _cartFaker = new Faker<Shoppingify.Cart.Domain.Cart>()
            .RuleFor(x => x.Id, f => new CartId(f.Random.Guid()))
            .RuleFor(x => x.Name, f => f.Random.String2(10))
            .RuleFor(x => x.CartOwnerId, f => new CartOwnerId(f.Random.Guid().ToString()))
            .RuleFor(x => x.CartItems, f => f.Make(3, () => new CartItem
            {
                Product = new ProductId(f.Random.Guid()),
                Quantity = f.Random.Int(1, 10),
                Status = f.PickRandom<CartItemStatus>()
            }));

        _cartItemFaker = new Faker<CartItem>()
            .RuleFor(x => x.Product, f => new ProductId(f.Random.Guid()))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(x => x.Status, f => f.PickRandom<CartItemStatus>());

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            })
        );

        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public async Task GetCarts_ReturnsOk(int cartCount)
    {
        // Arrange

        var carts = _cartFaker.Generate(cartCount);
        _cartApplicationServiceMock.Setup(x => x.GetCarts(It.IsAny<string>())).ReturnsAsync(carts);

        // Act
        var result = await _cartController.GetCarts();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(carts, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetCarts_UidNull_ReturnsBadRequest()
    {
        // Arrange
        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _cartController.GetCarts();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetActiveCart_WithCart_ReturnsOk()
    {
        // Arrange

        var cart = _cartFaker.Generate();
        _cartApplicationServiceMock.Setup(x => x.GetActiveCart(It.IsAny<string>())).ReturnsAsync(cart);

        // Act
        var result = await _cartController.GetActiveCart();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(cart, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetActiveCart_WithoutCart_ReturnsNoContent()
    {
        // Arrange

        _cartApplicationServiceMock.Setup(x => x.GetActiveCart(It.IsAny<string>()))
            .ReturnsAsync((Shoppingify.Cart.Domain.Cart?)null);

        // Act
        var result = await _cartController.GetActiveCart();

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetActiveCart_UidNull_ReturnsBadRequest()
    {
        // Arrange
        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _cartController.GetActiveCart();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateCart_WithValidData_ReturnsCreated()
    {
        // Arrange

        var cartId = Guid.NewGuid();
        var name = "My cart";
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock
            .Setup(x => x.CreateCart(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<CartItem>>()))
            .ReturnsAsync(new CartId(cartId));

        // Act
        var result = await _cartController.CreateCart(new CreateCartCommand(name, items));

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CreateCart_WithActiveCart_ReturnsBadRequest()
    {
        // Arrange

        var name = "My cart";
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.CreateCart(It.IsAny<string>(), name, items))
            .Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.CreateCart(new CreateCartCommand(name, items));

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCart_ReturnsNull_ReturnsServerError()
    {
        // Arrange

        var name = "My cart";
        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.CreateCart(It.IsAny<string>(), name, items))
            .ReturnsAsync((CartId?)null);

        // Act
        var result = await _cartController.CreateCart(new CreateCartCommand(name, items));

        // Assert
        Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, ((StatusCodeResult)result).StatusCode);
    }

    [Fact]
    public async Task CreateCart_UidNull_ReturnsBadRequest()
    {
        // Arrange
        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _cartController.CreateCart(new CreateCartCommand("My cart", new List<CartItem>()));

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateCartList_WithValidData_ReturnsOk()
    {
        // Arrange

        var items = _cartItemFaker.Generate(3);

        // Act
        var result = await _cartController.UpdateCartList(items);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateCartList_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange

        var items = _cartItemFaker.Generate(3);
        _cartApplicationServiceMock.Setup(x => x.UpdateCartList(It.IsAny<string>(), items))
            .Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.UpdateCartList(items);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCartList_UidNull_ReturnsBadRequest()
    {
        // Arrange
        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _cartController.UpdateCartList(new List<CartItem>());

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CompleteCart_WithValidData_ReturnsOk()
    {
        // Act
        var result = await _cartController.CompleteCart();

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CompleteCart_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange
        _cartApplicationServiceMock.Setup(x => x.CompleteCart(It.IsAny<string>())).Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.CompleteCart();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CompleteCart_UidNull_ReturnsBadRequest()
    {
        // Arrange
        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _cartController.CompleteCart();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CancelCart_WithValidData_ReturnsOk()
    {
        // Act
        var result = await _cartController.CancelCart();

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CancelCart_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange

        _cartApplicationServiceMock.Setup(x => x.CancelCart(It.IsAny<string>())).Throws<InvalidOperationException>();

        // Act
        var result = await _cartController.CancelCart();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CancelCart_UidNull_ReturnsBadRequest()
    {
        // Arrange
        _cartController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _cartController.CancelCart();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}