﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Cable
{
    [Key]
    public Guid CableId { get; set; }
    [Required]
    public string CableName { get; set; } = null!;
    public int Price { get; set; }
    public string Image { get; set; } = string.Empty;
    [Column(TypeName = "jsonb")]
    public string CableDescription { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Guid CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}