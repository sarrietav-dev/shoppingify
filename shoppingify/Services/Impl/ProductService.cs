using shoppingify.Entities;
using shoppingify.Repositories;

namespace shoppingify.Services.Impl;

class ProductService : IProductService
{
    private readonly ShoppingContext _context;
    private readonly IProductRepository _repository;
    
    public ProductService(ShoppingContext context, IProductRepository repository)
    {
        _context = context;
        _repository = repository;
    }
    
    public IEnumerable<Product> GetProductsAsync()
    {
        return _repository.GetAllProducts();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _repository.GetProductById(id);
    }

    public async Task<Product> CreateProductAsync(ProductInput product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Note = product.Note,
            Category = product.Category,
            Image = product.Image
        };
        await _repository.CreateProductAsync(newProduct);
        return newProduct;
    }

    public async Task DeleteProductAsync(int id)
    {
        await _repository.DeleteProduct(id);
    }
}