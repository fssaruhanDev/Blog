namespace Blog.Api.Application.Queries.Category.GetPublicCategories;

public class GetPublicCategoriesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; }
    public int PostCount { get; set; }
}