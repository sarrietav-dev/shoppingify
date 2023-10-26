using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using Shoppingify.Cart.Application;
using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartApplicationServiceTests
{
    private readonly ICartApplicationService _cartApplicationService;
    private readonly Faker<CartItemDto> _cartItemFaker;
    private readonly Faker<CartOwner> _cartOwnerFaker;
    private readonly Faker<CartDto> _cartFakerDto;
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
        _cartItemFaker = new Faker<CartItemDto>()
            .RuleFor(x => x.ProductId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(x => x.Status, f => f.PickRandom<CartItemStatus>().ToString());
        _cartFakerDto = new Faker<CartDto>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.Name, f => f.Random.String2(10))
            .RuleFor(x => x.CartOwnerId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CartItems, _ => _cartItemFaker.Generate(3));
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

        var cartId = await _cartApplicationService.CreateCart(cartOwnerId.Value, "My Cart", cartItems);
        
        Assert.NotNull(cartId);
        _cartOwnerRepositoryMock.Verify(x => x.Get(cartOwnerId), Times.Once);
        _cartOwnerRepositoryMock.Verify(x => x.Add(It.IsAny<CartOwner>()), Times.Once);
    }

    [Fact]
    public async void GetActiveCart_CartOwnerWithActiveCart_ReturnsCart()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        var createdCart = cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);
        _cartRepositoryMock.Setup(x => x.Get(createdCart.Id)).ReturnsAsync(createdCart);

        var cart = await _cartApplicationService.GetActiveCart(cartOwner.Id.Value);

        Assert.Equal(cartOwner.ActiveCart?.Value.ToString(), cart?.Id);
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
        var cart = cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);
        _cartRepositoryMock.Setup(x => x.Get(cart.Id)).ReturnsAsync(cart);

        var cartItems = _cartItemFaker.Generate(5);

        await _cartApplicationService.UpdateCartList(cartOwner.Id.Value, cartItems);

        var cartItemsMapped = cartItems.Select(ci => ci.ToCartItem());

        Assert.Equal(cartItemsMapped, cart.CartItems);
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
        var cart = cartOwner.CreateCart("My Cart");
        cart.Complete();

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
        var cart = cartOwner.CreateCart("My Cart");
        cart.Cancel();

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
        var cart = cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);
        _cartRepositoryMock.Setup(x => x.Get(cart.Id)).ReturnsAsync(cart);

        await _cartApplicationService.CompleteCart(cartOwner.Id.Value);

        Assert.Null(cartOwner.ActiveCart);
        Assert.Equal(CartState.Completed, cart.State);
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
        var cart = cartOwner.CreateCart("My Cart");
        cart.Complete();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartApplicationService.CompleteCart(cartOwner.Id.Value));
    }

    [Fact]
    public async void CancelCart_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        var cart = cartOwner.CreateCart("My Cart");

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);
        _cartRepositoryMock.Setup(x => x.Get(cart.Id)).ReturnsAsync(cart);

        await _cartApplicationService.CancelCart(cartOwner.Id.Value);

        Assert.Null(cartOwner.ActiveCart);
        Assert.Equal(CartState.Canceled, cart.State);
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
        var cart = cartOwner.CreateCart("My Cart");
        cart.Cancel();

        _cartOwnerRepositoryMock.Setup(x => x.Get(cartOwner.Id)).ReturnsAsync(cartOwner);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _cartApplicationService.CancelCart(cartOwner.Id.Value));
    }

    [Fact]
    public async void GetCarts_WithValidData_Successfully()
    {
        var cartOwner = _cartOwnerFaker.Generate();
        var cartFaker = new Faker<Shoppingify.Cart.Domain.Cart>()
            .RuleFor(x => x.Id, f => new CartId(f.Random.Guid()))
            .RuleFor(x => x.Name, f => f.Random.String2(10))
            .RuleFor(x => x.CartOwnerId, f => cartOwner.Id)
            .RuleFor(x => x.CartItems, f => f.Make(10, i =>
            {
                var cartItem = new CartItem
                {
                    Product = new ProductId(Guid.NewGuid()),
                    Quantity = f.Random.Int(1, 10),
                    Status = f.PickRandom<CartItemStatus>()
                };
                return cartItem;
            }));
        var fakeCarts = cartFaker.Generate(10);

        _cartRepositoryMock.Setup(x => x.GetAll(It.IsAny<string>())).ReturnsAsync(fakeCarts);

        var carts = await _cartApplicationService.GetCarts(cartOwner.Id.Value);

        var cartsDto = fakeCarts.Select(CartDto.ToCartDto);

        Assert.Equal(cartsDto.Count(), carts.Count());
    }
}