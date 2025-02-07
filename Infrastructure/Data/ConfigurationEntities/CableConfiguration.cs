using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ConfigurationEntities;

public class CableConfiguration : IEntityTypeConfiguration<Cable>
{
    public void Configure(EntityTypeBuilder<Cable> builder)
    {
        builder.HasKey(c => c.CableId);
        builder.HasOne(c => c.Category)
            .WithMany(cat => cat.Cables)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}