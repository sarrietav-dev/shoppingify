using shoppingify.Entities;
using shoppingify.Repositories;

namespace shoppingify.Services.Impl;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public ShoppingCartService(IShoppingCartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }
    
    public void AddItemToCart(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = _productRepository.GetProductById(productId);
        
        cart.AddItem(product);
        
        _cartRepository.SaveCart();
    }

    public void RemoveItemFromCart(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = _productRepository.GetProductById(productId);
        
        cart.RemoveItem(product);
        
        _cartRepository.SaveCart();
    }

    public void EditItemsCount(string cartId, IEnumerable<SetItemCountInput> items)
    {
        var cart = _cartRepository.GetCart(cartId);
        foreach (var item in items)
        {
            var product = _productRepository.GetProductById(item.ProductId);
            cart.ChangeItemQuantity(product, item.Count);
        }
        _cartRepository.SaveCart();
    }

    public void CheckItem(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = _productRepository.GetProductById(productId);
        
        cart.CheckItem(product);
        
        _cartRepository.SaveCart();
    }

    public void UncheckItem(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = _productRepository.GetProductById(productId);
        
        cart.UncheckItem(product);
        
        _cartRepository.SaveCart();
    }

    public void SaveCart(SaveCartInput cart)
    {
        var newCart = new ShoppingCart();
        var products = _productRepository.GetProductsById(cart.Items.Select(i => i.ProductId)).ToList();
        
        foreach (var item in cart.Items)
        {
            var product = products.First(p => p.Id == Guid.Parse(item.ProductId));
            newCart.AddItem(product, item.Count);
        }

        _cartRepository.CreateCart(newCart);
    }
}