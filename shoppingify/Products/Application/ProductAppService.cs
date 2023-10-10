using Shoppingify.Products.Application.Commands;
using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Application;

public class ProductApplicationService : IProductApplicationService
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductApplicationService(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Product?> Get(Guid productId)
    {
        return await _repository.Get(new ProductId(productId));
    }

    public async Task<IEnumerable<Product>> GetAll(string productOwnerId)
    {
        return await _repository.GetAll(new ProductOwner(productOwnerId));
    }

    public async Task Add(string ownerId, AddProductCommand product)
    {
        var newProduct = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner(ownerId),
            Name = product.Name,
            Note = product.Note,
            Category = product.Category,
            Image = product.Image
        };

        await _repository.Add(newProduct);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Delete(Guid productId)
    {
        await _repository.Delete(new ProductId(productId));
        await _unitOfWork.SaveChangesAsync();
    }
}