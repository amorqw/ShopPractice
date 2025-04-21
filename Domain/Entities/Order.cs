using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Order
{
    [Key]
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string ShippingAddress { get; set; } = string.Empty;
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}