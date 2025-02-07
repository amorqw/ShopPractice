using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ConfigurationEntities;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {

        builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasMany(c => c.Cables)
            .WithOne(cable => cable.Category)
            .HasForeignKey(cable => cable.CategoryId) 
            .OnDelete(DeleteBehavior.Restrict);
    }
}