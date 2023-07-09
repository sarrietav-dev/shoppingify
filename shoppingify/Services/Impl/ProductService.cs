using shoppingify;
using shoppingify.Entities;
using shoppingify.Services;

class ProductService : IProductService
{
    private ShoppingContext _context;
    
    public ProductService(ShoppingContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Product> GetProductsAsync()
    {
        return _context.Products;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id) ?? throw new KeyNotFoundException();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(int id)
    {
        _context
            .Products.Remove(await _context.Products.FindAsync(id) ?? throw new KeyNotFoundException());
        await _context.SaveChangesAsync();
    }
}