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
        var cartOwner = await GetCartOwner(cartOwnerId);

        try
        {
            var cart = cartOwner.CreateCart(name);
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
        var cartOwner = await GetCartOwner(cartOwnerId);

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

    public async Task UpdateCartList(string ownerId, IEnumerable<CartItem> cartItems)
    {
        var cartOwner = await GetCartOwner(ownerId);
        
        if (cartOwner.ActiveCartId is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", ownerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }
        
        var cart = await GetCart(cartOwner.ActiveCartId.Value);

        try
        {
            cart.UpdateList(cartItems);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning("Cart {Id} is not active so cannot update", cart.Id);
            throw;
        }
    }

    public async Task CompleteCart(string cartOwnerId)
    {

        var cartOwner = await GetCartOwner(cartOwnerId: cartOwnerId);
        
        if (cartOwner.ActiveCartId is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        var cart = await GetCart(cartId: cartOwner.ActiveCartId.Value);

        try
        {
            cart.Complete();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning("Cart {Id} is not active so cannot complete", cart.Id.Value);
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
    
    private async Task<CartOwner> GetCartOwner(string cartOwnerId)
    {
        var cartOwner = await _cartOwnerRepository.Get(new CartOwnerId(cartOwnerId));
        if (cartOwner != null) return cartOwner;
        _logger.LogWarning("Cart owner {Id} not found", cartOwnerId);
        throw new InvalidOperationException("Cart owner not found");

    }
    
    private async Task<Domain.Cart> GetCart(Guid cartId)
    {
        var cart = await _cartRepository.Get(new CartId(cartId));
        if (cart != null) return cart;
        _logger.LogWarning("Cart {Id} not found", cartId);
        throw new InvalidOperationException("Cart not found");
    }
}