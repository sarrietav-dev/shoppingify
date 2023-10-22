using AutoMapper;
using Shoppingify.Cart.Domain;
using Shoppingify.Lib;

namespace Shoppingify.Tests.Mappers;

public class MapTests
{
    [Fact]
    public void Test_Mappers()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(AutoMapperProfile).Assembly);
        });
        
        config.AssertConfigurationIsValid();
    }
    
    [Fact]
    public void Test_ProductId_Maps()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(AutoMapperProfile).Assembly);
        });
        
        var mapper = config.CreateMapper();
        
        var productId = new ProductId(Guid.NewGuid());
        var productIdDto = mapper.Map<string>(productId);
        var productIdMapped = mapper.Map<ProductId>(productIdDto);
        
        Assert.Equal(productId.Value, productIdMapped.Value);
    }
}