namespace Blog.Common.Models.Queries.Category;

public class CategoryViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int Order { get; set; }
    public int PostCount { get; set; }
    public int ChildrenCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}