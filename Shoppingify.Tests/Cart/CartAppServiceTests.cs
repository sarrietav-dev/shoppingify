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
    private readonly Faker<Shoppingify.Cart.Domain.Cart> _cartFaker;
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
        _cartFaker = new Faker<Shoppingify.Cart.Domain.Cart>()
            .RuleFor(c => c.Id, f => new CartId(f.Random.Guid()))
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.CartOwnerId, f => new CartOwnerId(f.Random.AlphaNumeric(5)))
            .RuleFor(c => c.CartItems, f => _cartItemFaker.Generate(f.Random.Int(1, 10)));
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
    public async void CreateCart_WithValidDataAndCartItems_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        var cartItems = _cartItemFaker.Generate(5);

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await _cartApplicationService.CreateCart(cartOwner.Id.Value, "My Cart", cartItems);

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CreateCart_CartOwnerWithActiveCartAndCartItems_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");
        var cartItems = _cartItemFaker.Generate(5);

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.CreateCart(cartOwner.Id.Value, "My Cart", cartItems));

        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwner.Id), Times.Once);
    }

    [Fact]
    public async void CreateCart_CartOwnerIsNullAndCartItems_CreatesCartOwner()
    {
        var cartOwnerId = new CartOwnerId("y1281");
        var cartItems = _cartItemFaker.Generate(5);

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwnerId)).ReturnsAsync((CartOwner?)null);

        await _cartApplicationService.CreateCart(cartOwnerId.Value, "My Cart", cartItems);

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
    public async void CompleteCart_CompleteCartThrowsException_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");
        cartOwner.ActiveCart?.Complete();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.CompleteCart(cartOwner.Id.Value));
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

    [Fact]
    public async void CancelCart_CancelCartThrowsException_ThrowsException()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        cartOwner.CreateCart("My Cart");
        cartOwner.ActiveCart?.Cancel();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _cartApplicationService.CancelCart(cartOwner.Id.Value));
    }

    [Fact]
    public async void GetCarts_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        var fakeCarts = _cartFaker.Generate(10);

        _cartRepositoryMock.Setup(x => x.GetAll(It.IsAny<string>())).ReturnsAsync(fakeCarts);

        var carts = await _cartApplicationService.GetCarts(cartOwner.Id.Value);

        Assert.Equal(fakeCarts, carts);
    }
}