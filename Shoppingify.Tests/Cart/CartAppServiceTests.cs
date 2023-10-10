using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using Shoppingify.Cart.Application;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartApplicationServiceTests
{
    private readonly ICartApplicationService _cartApplicationService;
    private readonly Faker<CartItem> _cartItemFaker;
    private readonly Faker<CartOwner> _cartOwnerFaker;
    private readonly Mock<ICartOwnerRepository> _cartOwnerRepositoryMock;
    private readonly Mock<ICartRepository> _cartRepositoryMock;

    public CartApplicationServiceTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _cartOwnerRepositoryMock = new Mock<ICartOwnerRepository>();
        var loggerMock = new Mock<ILogger<CartApplicationService>>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).Returns(Task.CompletedTask);
        _cartApplicationService = new CartApplicationService(_cartRepositoryMock.Object,
            _cartOwnerRepositoryMock.Object, loggerMock.Object, unitOfWorkMock.Object);
        _cartOwnerFaker = new Faker<CartOwner>()
            .RuleFor(x => x.Id, f => new CartOwnerId(f.Random.AlphaNumeric(5)));
        _cartItemFaker = new Faker<CartItem>()
            .RuleFor(ci => ci.Product, f => new Product(f.Random.Guid(), f.Commerce.ProductName()))
            .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 10));
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

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.CreateCart(cartOwner.Id.Value, "My Cart"));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CreateCart_CartOwnerNotFound_CreatesCartOwner()
    {
        var cartOwnerId = new CartOwnerId("y1281");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwnerId)).ReturnsAsync((CartOwner?)null);

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

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync((CartOwner?)null);

        var cart = await _cartApplicationService.GetActiveCart(cartOwner.Id.Value);

        Assert.Null(cart);
        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void UpdateCartList_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        var cartItems = _cartItemFaker.Generate(5);

        await _cartApplicationService.UpdateCartList(cartOwner.Id.Value, cartItems);

        Assert.Equal(cartItems, cartOwner.ActiveCart?.CartItems);
        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void UpdateCartList_CartOwnerNotFound_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync((CartOwner?)null);

        var cartItems = _cartItemFaker.Generate(2);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.UpdateCartList(cartOwner.Id.Value, cartItems));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void UpdateCartList_CartOwnerWithoutActiveCart_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        var cartItems = _cartItemFaker.Generate(2);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.UpdateCartList(cartOwner.Id.Value, cartItems));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void UpdateCartList_CartOwnerWithNotActiveCart_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");
        cartOwner.ActiveCart?.Complete();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        var cartItems = _cartItemFaker.Generate(2);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.UpdateCartList(cartOwner.Id.Value, cartItems));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void UpdateCartList_CartOwnerWithCanceledCart_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");
        cartOwner.ActiveCart?.Cancel();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        var cartItems = _cartItemFaker.Generate(2);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.UpdateCartList(cartOwner.Id.Value, cartItems));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CompleteCart_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await _cartApplicationService.CompleteCart(cartOwner.Id.Value);

        _cartRepositoryMock.Verify(x => x.Add(It.IsAny<Shoppingify.Cart.Domain.Cart>()), Times.Once);
    }

    [Fact]
    public async void CompleteCart_CartOwnerNotFound_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync((CartOwner?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.CompleteCart(cartOwner.Id.Value));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CompleteCart_CartOwnerWithoutActiveCart_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.CompleteCart(cartOwner.Id.Value));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CancelCart_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await _cartApplicationService.CancelCart(cartOwner.Id.Value);

        _cartRepositoryMock.Verify(x => x.Add(It.IsAny<Shoppingify.Cart.Domain.Cart>()), Times.Once);
    }

    [Fact]
    public async void CancelCart_CartOwnerNotFound_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync((CartOwner?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _cartApplicationService.CancelCart(cartOwner.Id.Value));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CancelCart_CartOwnerWithoutActiveCart_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _cartApplicationService.CancelCart(cartOwner.Id.Value));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }
}