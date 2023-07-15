using shoppingify.Entities;

namespace shoppingify.Repositories;

public interface IProductRepository
{
    public Product GetProductById(string id);
    public IEnumerable<Product> GetProductsById(IEnumerable<string> ids);
}