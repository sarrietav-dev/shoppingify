using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Infrastructure.Persistence;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id.Value);
        builder.HasAlternateKey(x => x.Owner.Value);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasConversion(x => x.Value, x => new ProductId(x))
            .IsRequired();

        builder.Property(x => x.Owner)
            .HasColumnName("Owner")
            .HasConversion(x => x.Value, x => new ProductOwner(x))
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .IsRequired();

        builder.Property(x => x.Note);

        builder.Property(x => x.Category)
            .IsRequired();

        builder.Property(x => x.Image);
    }
}