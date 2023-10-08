using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using shoppingify.Cart.Application;
using shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartApplicationServiceTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<ICartOwnerRepository> _cartOwnerRepositoryMock;
    private readonly ICartApplicationService _cartApplicationService;
    private readonly Faker<CartOwner> _cartOwnerFaker;

    public CartApplicationServiceTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _cartOwnerRepositoryMock = new Mock<ICartOwnerRepository>();
        var _loggerMock = new Mock<ILogger<CartApplicationService>>();
        _cartApplicationService = new CartApplicationService(_cartRepositoryMock.Object, _cartOwnerRepositoryMock.Object, _loggerMock.Object);
        _cartOwnerFaker = new Faker<CartOwner>()
            .RuleFor(x => x.Id, f => new CartOwnerId(f.Random.AlphaNumeric(5)));
    }

    [Fact]
    public async void CreateCart_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await _cartApplicationService.CreateCart(cartOwner.Id.Value, "My Cart");

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CreateCart_CartOwnerWithActiveCart_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _cartApplicationService.CreateCart(cartOwner.Id.Value, "My Cart"));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CreateCart_CartOwnerNotFound_CreatesCartOwner() {
        var cartOwnerId = new CartOwnerId("y1281");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwnerId)).ReturnsAsync((CartOwner?) null);

        await _cartApplicationService.CreateCart(cartOwnerId.Value, "My Cart");

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwnerId), Times.Once);
        _cartOwnerRepositoryMock.Verify(x => x.Add(It.IsAny<CartOwner>()), Times.Once);
    }

    [Fact]
    public async void GetActiveCart_CartOwnerWithActiveCart_ReturnsCart()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        var cart = await _cartApplicationService.GetActiveCart(cartOwner.Id.Value);

        Assert.Equal(cartOwner.ActiveCart, cart);
        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void GetActiveCart_CartOwnerWithoutActiveCart_ReturnsNull()
    {
        var cartOwner = _cartOwnerFaker.Generate();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        var cart = await _cartApplicationService.GetActiveCart(cartOwner.Id.Value);

        Assert.Null(cart);
        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void GetActiveCart_CartOwnerNotFound_ReturnsNull()
    {
        var cartOwner = _cartOwnerFaker.Generate();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync((CartOwner?) null);

        var cart = await _cartApplicationService.GetActiveCart(cartOwner.Id.Value);

        Assert.Null(cart);
        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }
}