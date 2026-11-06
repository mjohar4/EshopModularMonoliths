using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configrations;

public class ShoppingCartConfigration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(e => e.UserName)
            .IsUnique();

        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(i => i.ShoppingCartId);
    }
}
