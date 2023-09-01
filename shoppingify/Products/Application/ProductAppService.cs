using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Application;

public class ProductApplicationService
{
    private readonly IProductRepository _repository;

    public ProductApplicationService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> Get(ProductId id)
    {
        return await _repository.Get(id);
    }

    public async Task<IEnumerable<Product>> GetAll(ProductOwner owner)
    {
        return await _repository.GetAll(owner);
    }

    public async Task Add(AddProductCommand product)
    {
        var newProduct = new Product {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner(Guid.Empty),
            Name = product.Name,
            Note = product.Note,
            Category = product.Category,
            Image = product.Image
        };

        await _repository.Add(newProduct);
    }

    public async Task Delete(ProductId productId)
    {
        await _repository.Delete(productId);
    }
}