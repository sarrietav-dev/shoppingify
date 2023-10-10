using Shoppingify.Products.Domain;

namespace Shoppingify.Tests.Products;

public class ProductsTest
{
    [Fact]
    public void Product_CreateProduct_ReturnsCorrectValues()
    {
        var product = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.Equal("name", product.Name);
        Assert.Equal("category", product.Category);
        Assert.Equal("note", product.Note);
        Assert.Equal("image", product.Image);
    }

    [Fact]
    public void Product_TwoProductsWithTheSameId_AreEqual()
    {
        var id = new ProductId(Guid.NewGuid());
        var product1 = new Product
        {
            Id = id,
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        var product2 = new Product
        {
            Id = id,
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.Equal(product1, product2);
    }
    
    [Fact]
    public void Product_TwoProductsWithDifferentId_AreNotEqual()
    {
        var product1 = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        var product2 = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.NotEqual(product1, product2);
    }
    
    [Fact]
    public void Product_TwoProductsWithTheSameId_HaveTheSameHashCode()
    {
        var id = new ProductId(Guid.NewGuid());
        var product1 = new Product
        {
            Id = id,
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        var product2 = new Product
        {
            Id = id,
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.Equal(product1.GetHashCode(), product2.GetHashCode());
    }
    
    [Fact]
    public void Product_TwoProductsWithDifferentId_HaveDifferentHashCode()
    {
        var product1 = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        var product2 = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.NotEqual(product1.GetHashCode(), product2.GetHashCode());
    }
    
    [Fact]
    public void Equals_OneNullProduct_ReturnsFalse()
    {
        var product = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.False(product.Equals(null));
    }
    
    [Fact]
    public void Equals_OneProductWithDifferentType_ReturnsFalse()
    {
        var product = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner("y1281"),
            Name = "name",
            Category = "category",
            Note = "note", Image = "image"
        };
        Assert.False(product.Equals(new object()));
    }
}