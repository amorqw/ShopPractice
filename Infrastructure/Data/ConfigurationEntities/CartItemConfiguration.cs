using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ConfigurationEntities;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        
        builder.HasKey(ci => ci.Id);
        
        
        builder.HasOne(ci => ci.Cable)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CableId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(ci => ci.Order)
            .WithMany(o => o.CartItems)
            .HasForeignKey(ci => ci.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(ci => ci.User)
            .WithMany()
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        
        builder.Property(ci => ci.Quantity)
            .IsRequired();
            
        builder.Property(ci => ci.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(ci => ci.Status)
            .IsRequired()
            .HasConversion<string>();
    }
} 