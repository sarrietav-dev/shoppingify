using shoppingify.Entities;

namespace shoppingify.Repositories;

public interface IProductRepository
{
    public Task<Product> GetProductById(string id);
    public IEnumerable<Product> GetProductsById(IEnumerable<string> ids);
    public IEnumerable<Product> GetAllProducts();
    public Task CreateProductAsync(Product newProduct);
    public Task DeleteProduct(int id);
}