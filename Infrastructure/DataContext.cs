using Domain.Entities;
using Infrastructure.Data.ConfigurationEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cable> Cables { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new CableConfiguration());
        builder.ApplyConfiguration(new CartItemConfiguration());
    }
}