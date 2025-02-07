using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; }

    public string Title { get; set; } = string.Empty;
    public List<Cable> Cables { get; set; } = new List<Cable>();
}