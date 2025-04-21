using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ConfigurationEntities;

public class CableConfiguration : IEntityTypeConfiguration<Cable>
{
    public void Configure(EntityTypeBuilder<Cable> builder)
    {
        builder.HasKey(c => c.CableId);

        builder.Property(c => c.CableName)
            .IsRequired();

        builder.Property(c => c.Price)
            .IsRequired();

        builder.Property(c => c.CableDescription)
            .IsRequired();

        builder.HasOne(c => c.Category)
            .WithMany(cat => cat.Cables)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cable)
            .HasForeignKey(ci => ci.CableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}