namespace Blog.Common.Models.Event.Category;

public class UpdateCategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime UpdatedDate { get; set; }
}