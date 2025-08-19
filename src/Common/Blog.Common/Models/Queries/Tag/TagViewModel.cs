namespace Blog.Common.Models.Queries.Tag;

public class TagViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int ProjectCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}