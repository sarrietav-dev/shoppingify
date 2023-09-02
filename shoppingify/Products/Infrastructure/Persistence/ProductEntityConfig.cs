using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shoppingify.Products.Domain;

namespace shoppingify.Products.Infrastructure.Persistence;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.Owner);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ProductId(x))
            .IsRequired();

        builder.Property(x => x.Owner)
            .HasConversion(x => x.Value, x => new ProductOwner(x))
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Note);

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.Image);
    }
}