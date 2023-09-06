using shoppingify.Cart.Domain;
using InvalidOperationException = System.InvalidOperationException;

namespace shoppingify.Cart.Application;

public class CartApplicationService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartOwnerRepository _cartOwnerRepository;
    private readonly ILogger _logger;

    public CartApplicationService(ICartRepository cartRepository, ICartOwnerRepository cartOwnerRepository,
        ILogger<CartApplicationService> logger)
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
        {
            _logger.LogWarning("Cart owner {Id} not found", cartOwnerId);
            throw new InvalidOperationException("Cart owner not found");
        }

        if (cartOwner.ActiveCartId is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        var cart = await _cartRepository.Get(cartOwner.ActiveCartId);

        if (cart != null) return cart;

        _logger.LogWarning("Cart {Id} not found", cartOwner.ActiveCartId);
        throw new InvalidOperationException("Cart not found");
    }

    public async Task UpdateCartList(Guid cartId, IEnumerable<CartItem> cartItems)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));

        if (cart == null)
        {
            _logger.LogWarning("Cart {Id} not found", cartId);
            throw new InvalidOperationException("Cart not found");
        }

        try
        {
            cart.UpdateList(cartItems);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning("Cart {Id} is not active so cannot update", cartId);
            throw;
        }
    }

    public async Task CompleteCart(Guid cartId)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));

        if (cart == null)
        {
            _logger.LogWarning("Cart {Id} not found", cartId);
            throw new InvalidOperationException("Cart not found");
        }

        try
        {
            cart.Complete();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning("Cart {Id} is not active so cannot complete", cartId);
            throw;
        }
    }

    public async Task CancelCart(Guid cartId)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));

        if (cart == null)
        {
            _logger.LogWarning("Cart {Id} not found", cartId);
            throw new InvalidOperationException("Cart not found");
        }

        try
        {
            cart.Cancel();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning("Cart {Id} is not active so cannot cancel", cartId);
            throw;
        }
    }
}