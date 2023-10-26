using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application;

public class CartApplicationService : ICartApplicationService
{
    private readonly ICartOwnerRepository _cartOwnerRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CartApplicationService(ICartRepository cartRepository, ICartOwnerRepository cartOwnerRepository,
        ILogger<CartApplicationService> logger, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartOwnerRepository = cartOwnerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<CartId?> CreateCart(string cartOwnerId, string name)
    {
        var cartOwner = await GetCartOwner(cartOwnerId);

        try
        {
            if (cartOwner is null)
            {
                cartOwner = new CartOwner
                {
                    Id = new CartOwnerId(cartOwnerId)
                };
                var cart = cartOwner.CreateCart(name);
                await _cartOwnerRepository.Add(cartOwner);
                await _cartRepository.Add(cart);
                return cart.Id;
            }

            var createdCart = cartOwner.CreateCart(name);

            _logger.LogInformation("Cart {Id} created for cart owner {CartOwnerId}", cartOwner.ActiveCart,
                cartOwner.Id);

            await _unitOfWork.SaveChangesAsync();

            return createdCart.Id;
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Cart owner {CartOwnerId} already has an active cart", cartOwnerId);
            throw;
        }
    }

    public async Task<CartId?> CreateCart(string cartOwnerId, string name, IEnumerable<CartItemDto> cartItems)
    {
        var cartOwner = await GetCartOwner(cartOwnerId);
        var items = cartItems.Select(ci => ci.ToCartItem());

        try
        {
            if (cartOwner is null)
            {
                cartOwner = new CartOwner
                {
                    Id = new CartOwnerId(cartOwnerId)
                };
                var cart = cartOwner.CreateCart(name, items);
                await _cartOwnerRepository.Add(cartOwner);
                await _cartRepository.Add(cart);
                await _unitOfWork.SaveChangesAsync();
                return cart.Id;
            }

            var createdCart = cartOwner.CreateCart(name, items);
            await _cartRepository.Add(createdCart);

            _logger.LogInformation("Cart {Id} created for cart owner {CartOwnerId}", cartOwner.ActiveCart,
                cartOwner.Id);

            await _unitOfWork.SaveChangesAsync();

            return createdCart.Id;
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Cart owner {CartOwnerId} already has an active cart", cartOwnerId);
            throw;
        }
    }

    public async Task<CartDto?> GetActiveCart(string cartOwnerId)
    {
        var cartOwner = await GetCartOwner(cartOwnerId);

        if (cartOwner is null) return null;

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            return null;
        }

        var cart = await _cartRepository.Get(cartOwner.ActiveCart);

        if (cart is not null) return CartDto.ToCartDto(cart);

        _logger.LogError("Cart {Id} from Owner {OwnerId} not found", cartOwner.ActiveCart, cartOwner.Id);
        return null;
    }

    public async Task UpdateCartList(string ownerId, IEnumerable<CartItemDto> cartItems)
    {
        var cartOwner = await GetCartOwner(ownerId);

        if (cartOwner is null) throw new InvalidOperationException("Cart owner not found");

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", ownerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        var cart = await _cartRepository.Get(cartOwner.ActiveCart);

        if (cart is null)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} not found", cartOwner.ActiveCart, ownerId);
            throw new InvalidOperationException("Cart not found");
        }

        try
        {
            var items = cartItems.Select(ci => ci.ToCartItem());
            cart.UpdateList(items);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} is not active so cannot update", cart.Id, ownerId);
            throw;
        }
    }

    public async Task CompleteCart(string cartOwnerId)
    {
        var cartOwner = await GetCartOwner(cartOwnerId);

        if (cartOwner is null) throw new InvalidOperationException("Cart owner not found");

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        var cart = await _cartRepository.Get(cartOwner.ActiveCart);

        if (cart is null)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} not found", cartOwner.ActiveCart, cartOwner.Id);
            throw new InvalidOperationException("Cart not found");
        }

        try
        {
            // TODO: Make this eventually consistent. Publish an event and update the cart.
            cartOwner.CompleteCart();
            cart.Complete();
            await _unitOfWork.SaveChangesAsync();
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} is not active so cannot complete",
                cartOwner.ActiveCart, cartOwner.Id);
            throw;
        }
    }

    public async Task CancelCart(string cartOwnerId)
    {
        var cartOwner = await GetCartOwner(cartOwnerId);

        if (cartOwner is null) throw new InvalidOperationException("Cart owner not found");

        if (cartOwner.ActiveCart is null)
        {
            _logger.LogWarning("Cart owner {Id} has no active cart", cartOwnerId);
            throw new InvalidOperationException("Cart owner has no active cart");
        }

        var cart = await _cartRepository.Get(cartOwner.ActiveCart);

        if (cart is null)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} not found", cartOwner.ActiveCart, cartOwner.Id);
            throw new InvalidOperationException("Cart not found");
        }

        try
        {
            cartOwner.CancelCart();
            cart.Cancel();
            await _unitOfWork.SaveChangesAsync();
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Cart {Id} from Owner {OwnerId} is not active so cannot cancel",
                cartOwner.ActiveCart, cartOwner.Id);
            throw;
        }
    }

    public async Task<IEnumerable<CartDto>> GetCarts(string cartOwnerId)
    {
        var carts = await _cartRepository.GetAll(cartOwnerId);
        return carts.Select(CartDto.ToCartDto);
    }

    private async Task<CartOwner?> GetCartOwner(string cartOwnerId)
    {
        var cartOwner = await _cartOwnerRepository.Get(new CartOwnerId(cartOwnerId));
        _logger.LogWarning("Cart owner {Id} not found", cartOwnerId);
        return cartOwner;
    }
}