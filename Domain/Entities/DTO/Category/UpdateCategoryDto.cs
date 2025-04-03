namespace Domain.Entities.DTO.Category;

public class UpdateCategoryDto
{
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
}