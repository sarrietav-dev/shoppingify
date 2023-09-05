using shoppingify.Cart.Domain;

namespace shoppingify.Cart.Application;

public class CartApplicationService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartOwnerRepository _cartOwnerRepository;
    private readonly ILogger _logger;

    public CartApplicationService(ICartRepository cartRepository, ICartOwnerRepository cartOwnerRepository,
        ILogger logger)
    {
        _cartRepository = cartRepository;
        _cartOwnerRepository = cartOwnerRepository;
        _logger = logger;
    }

    public async Task CreateCart(string cartOwnerId, string name)
    {
        var cartOwner = await _cartOwnerRepository.Get(new CartOwnerId(cartOwnerId));
        if (cartOwner == null)
        {
            var invalidOperationException = new InvalidOperationException("Cart owner not found");
            _logger.LogWarning(invalidOperationException, "Cart owner with {Id} not found", cartOwnerId);
            throw invalidOperationException;
        }

        try
        {
            var cart = cartOwner.CreateCart(name, null);
            await _cartRepository.Add(cart);
            _logger.LogInformation("Cart {Id} created for cart owner {CartOwnerId}", cart.Id, cartOwnerId);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Cart owner {CartOwnerId} already has an active cart", cartOwnerId);
            throw;
        }
    }

    public async Task<Domain.Cart?> GetActiveCart(string cartOwnerId)
    {
        var cartOwner = await _cartOwnerRepository.Get(new CartOwnerId(cartOwnerId));
        if (cartOwner == null)
            throw new InvalidOperationException("Cart owner not found");

        if (cartOwner.ActiveCartId is null)
            throw new InvalidOperationException("Cart owner does not have an active cart");

        var cart = await _cartRepository.Get(cartOwner.ActiveCartId);
        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        return cart;
    }

    public async Task UpdateCartList(Guid cartId, IEnumerable<CartItem> cartItems)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));

        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        cart.UpdateList(cartItems);
    }

    public async Task CompleteCart(Guid cartId)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));

        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        cart.Complete();
    }

    public async Task CancelCart(Guid cartId)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));

        if (cart == null)
            throw new InvalidOperationException("Cart not found");

        cart.Cancel();
    }
}