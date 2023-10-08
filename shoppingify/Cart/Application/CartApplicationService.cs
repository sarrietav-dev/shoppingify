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
            cartOwner.CreateCart(name);
            _logger.LogInformation("Cart {Id} created for cart owner {CartOwnerId}", cartOwner.ActiveCart?.Id,
                cartOwner.Id);
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

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
        }

        return cartOwner.ActiveCart;
    }

    public async Task UpdateCartList(string ownerId, IEnumerable<CartItem> cartItems)
    {
        var cartOwner = await GetCartOwner(ownerId);

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", ownerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        var cart = cartOwner.ActiveCart;

        try
        {
            cart.UpdateList(cartItems);
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} is not active so cannot update", cart.Id, ownerId);
            throw;
        }
    }

    public async Task CompleteCart(string cartOwnerId)
    {
        var cartOwner = await GetCartOwner(cartOwnerId: cartOwnerId);

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        try
        {
            var cart = cartOwner.CompleteCart();
            await _cartRepository.Add(cart);
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} is not active so cannot complete",
                cartOwner.ActiveCart.Id, cartOwner.Id);
            throw;
        }
    }

    public async Task CancelCart(string cartOwnerId)
    {
        var cartOwner = await GetCartOwner(cartOwnerId: cartOwnerId);

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        try
        {
            var cart = cartOwner.CancelCart();
            await _cartRepository.Add(cart);
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} is not active so cannot cancel",
                cartOwner.ActiveCart.Id, cartOwner.Id);
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

    public async Task<IEnumerable<Domain.Cart>> GetCarts(string cartOwnerId)
    {
        return await _cartRepository.GetAll(cartOwnerId);
    }
}