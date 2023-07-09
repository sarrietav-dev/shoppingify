using shoppingify.Entities;

namespace shoppingify.Services;

interface IProductService
{
    IEnumerable<Product> GetProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task DeleteProductAsync(int id);
}