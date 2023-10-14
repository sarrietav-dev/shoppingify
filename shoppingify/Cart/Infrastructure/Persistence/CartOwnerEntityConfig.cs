using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Infrastructure.Persistence;

public class CartOwnerEntityConfig : IEntityTypeConfiguration<CartOwner>
{
    public void Configure(EntityTypeBuilder<CartOwner> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, value => new CartOwnerId(value));
        builder.Property(c => c.ActiveCart)
            .HasConversion(id => id == null ? (Guid?)null : id.Value,
                value => value != null ? new CartId(value.Value) : null);
    }
}