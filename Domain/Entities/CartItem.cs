using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public enum ItemStatus
{
    InCart,
    InOrder
}

public class CartItem
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid CableId { get; set; }
    [ForeignKey("CableId")]
    public Cable Cable { get; set; }
    
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public ItemStatus Status { get; set; }
    
    public Guid? OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
    
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
} 