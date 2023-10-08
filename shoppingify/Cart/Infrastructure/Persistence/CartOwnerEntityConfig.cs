using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shoppingify.Cart.Domain;

namespace shoppingify.Cart.Infrastructure.Persistence;

public class CartOwnerEntityConfig : IEntityTypeConfiguration<CartOwner>
{
    public void Configure(EntityTypeBuilder<CartOwner> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, value => new CartOwnerId(value));
        builder.HasOne<Domain.Cart>(owner => owner.ActiveCart)
            .WithOne()
            .HasForeignKey<Domain.Cart>(cart => cart.CartOwnerId);
    }
}