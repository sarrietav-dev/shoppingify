﻿using shoppingify.Entities;

namespace shoppingify.Services;

public interface IProductService
{
    IEnumerable<Product> GetProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(ProductInput product);
    Task DeleteProductAsync(int id);
}

public record ProductInput(string Name, string Note, string Category, string Image);
