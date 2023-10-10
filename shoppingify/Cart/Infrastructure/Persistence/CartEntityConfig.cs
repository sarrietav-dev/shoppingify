using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Infrastructure.Persistence;

public class CartEntityConfig : IEntityTypeConfiguration<Domain.Cart>
{
    public void Configure(EntityTypeBuilder<Domain.Cart> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasAlternateKey(c => c.CartOwnerId);
        builder.OwnsMany<CartItem>(c => c.CartItems);

        builder.Property(c => c.Id)
            .HasConversion(c => c.Value, c => new CartId(c))
            .IsRequired();

        builder.Property(c => c.CartOwnerId)
            .HasConversion(c => c.Value, c => new CartOwnerId(c))
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.State)
            .HasConversion(c => c.ToString(), c => Enum.Parse<CartState>(c))
            .IsRequired();
    }
}